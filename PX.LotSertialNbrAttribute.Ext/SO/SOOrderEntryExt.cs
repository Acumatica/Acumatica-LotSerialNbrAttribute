using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PX.Data;
using PX.Objects.SO;

namespace PX.LotSertialNbrAttribute.Ext
{
    public class SOOrderEntryExt : PXGraphExtension<SOOrderEntry>
    {
        public override void Initialize()
        {
            this.Itemsearch.Cache.AllowInsert = false;
            this.Itemsearch.Cache.AllowDelete = false;

            PXUIFieldAttribute.SetEnabled(Itemsearch.Cache, null, false);
            PXUIFieldAttribute.SetEnabled<ItemLotSerialSelected.selected>(Itemsearch.Cache, null, true);
            PXUIFieldAttribute.SetEnabled<ItemLotSerialSelected.qtySelected>(Itemsearch.Cache, null, true);
        }

        #region DataViews

        public PXFilter<ItemLotSerialAdvSearchFilter> ItemFilter;

        [PXFilterable]
        public PXSelect<ItemLotSerialSelected, Where<ItemLotSerialSelected.lotSerClassID,
                Equal<Current<ItemLotSerialAdvSearchFilter.itemLotSerialClass>>>> Itemsearch;

        #endregion

        #region Actions (buttons)

        public PXAction<SOOrder> advItemSearch;
        [PXUIField(DisplayName = "Item Lot/Serial Search", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXLookupButton]
        public virtual IEnumerable AdvItemSearch(PXAdapter adapter)
        {
            if (Itemsearch.AskExt((graph, viewname) =>
            {
                graph.Views[ItemFilter.View.Name].Cache.Clear();
                graph.Views[viewname].Cache.Clear();
                graph.Views[viewname].Cache.ClearQueryCache();
                graph.Views[viewname].ClearDialog();
            }, true) != WebDialogResult.OK) return adapter.Get();

            return AddInvItemSelByAdvFilter(adapter);
        }

        public PXAction<SOOrder> addInvItemSelByAdvFilter;
        [PXUIField(DisplayName = "Add", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXLookupButton]
        public virtual IEnumerable AddInvItemSelByAdvFilter(PXAdapter adapter)
        {
            foreach (ItemLotSerialSelected line in Itemsearch.Cache.Cached)
            {
                if ((line.Selected == true) && (line.QtySelected > 0))
                {
                    List<SOLine> lines = Base.Transactions.Select().RowCast<SOLine>().ToList();
                    SOLine lineFound = lines.Where(x => x.InventoryID == line.InventoryID && x.SiteID == line.SiteID).FirstOrDefault();
                    if (lineFound == null)
                    {
                        PXCache cache = Base.Transactions.Cache;
                        SOLine newline = cache.CreateCopy(Base.Transactions.Insert()) as SOLine;
                        newline.SiteID = line.SiteID;
                        newline.InventoryID = line.InventoryID;
                        newline.SubItemID = line.SubItemID;
                        newline.UOM = line.SalesUnit;

                        newline = cache.CreateCopy(Base.Transactions.Update(newline)) as SOLine;
                        if (newline.RequireLocation != true)
                            newline.LocationID = null;

                        newline = cache.CreateCopy(Base.Transactions.Update(newline)) as SOLine;
                        newline.Qty = line.QtySelected;

                        Base.Transactions.Update(newline);

                        SOLineSplit spl = Base.splits.Current;
                        spl.LotSerialNbr = line.LotSerialNbr;
                        spl.IsAllocated = true;
                        spl.Qty = line.QtySelected;
                        Base.splits.Update(spl);
                    }
                    else
                    {
                        Base.Transactions.Current = lineFound;
                        Base.splits.Current = Base.splits.Select();

                        SOLineSplit spl = Base.splits.Current;
                        if (spl != null && !spl.IsAllocated.GetValueOrDefault(false))
                        {
                            spl.LotSerialNbr = line.LotSerialNbr;
                            spl.IsAllocated = true;
                            spl.Qty = line.QtySelected;
                            Base.splits.Update(spl);
                        }
                        else
                        {
                            SOLineSplit newsplit = new SOLineSplit();
                            newsplit.SubItemID = lineFound.SubItemID;
                            newsplit.LotSerialNbr = line.LotSerialNbr;
                            newsplit.ExpireDate = line.ExpireDate;
                            newsplit.UOM = lineFound.UOM;

                            newsplit = Base.splits.Insert(newsplit);
                            newsplit.IsAllocated = true;
                            newsplit.Qty = line.QtySelected;
                            newsplit = Base.splits.Update(newsplit);
                        }
                    }
                }
            }
            return adapter.Get();
        }

        #endregion

        #region EventHandlers

        protected void SOOrder_RowSelected(PXCache sender, PXRowSelectedEventArgs e, PXRowSelected baseEventHandler)
        {
            if (baseEventHandler != null)
                baseEventHandler(sender, e);

            advItemSearch.SetEnabled(Base.Transactions.Cache.AllowInsert);
        }

        protected virtual void ItemLotSerialAdvSearchFilter_RowUpdated(PXCache sender, PXRowUpdatedEventArgs e)
        {
            if (e.Row == null || e.OldRow == null) return;

            if (!sender.ObjectsEqual<ItemLotSerialAdvSearchFilter.itemLotSerialClass>(e.Row, e.OldRow))
            {
                foreach (PXAddAtttributeColumns attrib in
                    Itemsearch.Cache.GetAttributes("LotSerialAttributes").OfType<PXAddAtttributeColumns>())
                {
                    attrib.InvokeCacheAttached(Itemsearch.Cache);
                }
            }
        }

        protected virtual void ItemLotSerialAdvSearchFilter_RowSelected(PXCache sender, PXRowSelectedEventArgs e)
        {
            if (e.Row == null) return;
            ItemLotSerialAdvSearchFilter filterData = (ItemLotSerialAdvSearchFilter)e.Row;
            PXUIFieldAttribute.SetEnabled<ItemLotSerialAdvSearchFilter.searchString>(sender, filterData, !String.IsNullOrEmpty(filterData.ItemLotSerialClass));
        }

        #endregion
    }
}