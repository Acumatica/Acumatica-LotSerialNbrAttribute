using System.Linq;
using PX.Data;
using System;
using System.Collections.Generic;
using PX.Objects.PO;
using PX.Objects.IN;

namespace PX.LotSertialNbrAttribute.Ext
{
    public class POReceiptEntryPXExt : PXGraphExtension<POReceiptEntry>
    {
        [PXCopyPasteHiddenView]
        public PXSelect<UsrInventoryLotSerialContainer, Where<UsrInventoryLotSerialContainer.inventoryID, Equal<Current<POReceiptLineSplit.inventoryID>>,
                                                            And<UsrInventoryLotSerialContainer.lotSerialNbr, Equal<Current<POReceiptLineSplit.lotSerialNbr>>>>>
                                            InventoryLotSerialContainer;

        [PXCopyPasteHiddenView]
        public CRAttributeListPXLotSerExt<UsrInventoryLotSerialContainer, POReceiptLineSplit, 
                                            POReceiptLineSplit.lotSerialNbr> Attributes;

        public void POReceiptLineSplit_RowInserted(PXCache sender, PXRowInsertedEventArgs e, PXRowInserted BaseInvoke)
        {
            if (BaseInvoke != null)
                BaseInvoke(sender, e);

            AssociateAttributeToLotSerial((POReceiptLineSplit)e.Row);
        }

        public void POReceiptLineSplit_RowUpdated(PXCache sender, PXRowUpdatedEventArgs e, PXRowUpdated BaseInvoke)
        {
            if (BaseInvoke != null)
                BaseInvoke(sender, e);

            AssociateAttributeToLotSerial((POReceiptLineSplit)e.Row);
        }

        public void AssociateAttributeToLotSerial(POReceiptLineSplit line)
        {
            if ((line == null) || (String.IsNullOrEmpty(line.LotSerialNbr))) return;

            UsrInventoryLotSerialContainer eContainer = InventoryLotSerialContainer.Search<UsrInventoryLotSerialContainer.inventoryID, 
                                                                                       UsrInventoryLotSerialContainer.lotSerialNbr>
                                                                                       (line.InventoryID, line.LotSerialNbr);
            if (eContainer != null)
            {
                InventoryLotSerialContainer.Current = eContainer;
            }
            else
            {
                UsrInventoryLotSerialContainer lotAtttrib = new UsrInventoryLotSerialContainer();
                lotAtttrib.InventoryID = line.InventoryID;
                lotAtttrib.LotSerialNbr = line.LotSerialNbr;
                lotAtttrib = InventoryLotSerialContainer.Insert(lotAtttrib);
            }
        }

        public void POReceiptLine_RowSelected(PXCache sender, PXRowSelectedEventArgs e, PXRowSelected BaseInvoke)
        {
            if (BaseInvoke != null)
                BaseInvoke(sender, e);

            if (e.Row == null) return;

            POReceiptLinePXExt lineExt = PXCache<POReceiptLine>.GetExtension<POReceiptLinePXExt>(e.Row as POReceiptLine);
            ApplyAttributes.SetVisible(lineExt.UsrLineLotSerTrack != INLotSerTrack.NotNumbered);
        }

        public PXAction<POReceipt> ApplyAttributes;
        [PXButton]
        [PXUIField(DisplayName = "Apply Attribute From First",
                   MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Update)]
        protected virtual void applyAttributes()
        {
            List<POReceiptLineSplit> splitList = Base.splits.Select().RowCast<POReceiptLineSplit>().Select(PXCache<POReceiptLineSplit>.CreateCopy).ToList();
            if (splitList.Count <= 1) return;
            POReceiptLineSplit firstSplit = splitList.First();
            UsrInventoryLotSerialContainer firstlotAttribs = PXSelect<UsrInventoryLotSerialContainer,
                                                                Where<UsrInventoryLotSerialContainer.inventoryID, Equal<Required<POReceiptLineSplit.inventoryID>>,
                                                                    And<UsrInventoryLotSerialContainer.lotSerialNbr, Equal<Required<POReceiptLineSplit.lotSerialNbr>>>>>
                                                                    .Select(Base, firstSplit.InventoryID, firstSplit.LotSerialNbr);
            foreach (POReceiptLineSplit split in splitList)
            {
                UsrInventoryLotSerialContainer lotAttribs = PXSelect<UsrInventoryLotSerialContainer,
                                                                    Where<UsrInventoryLotSerialContainer.inventoryID, Equal<Required<POReceiptLineSplit.inventoryID>>,
                                                                        And<UsrInventoryLotSerialContainer.lotSerialNbr, Equal<Required<POReceiptLineSplit.lotSerialNbr>>>>>
                                                                        .Select(Base, split.InventoryID, split.LotSerialNbr);
                if (firstlotAttribs.NoteID != lotAttribs.NoteID)
                    Attributes.CopyAllAttributes(lotAttribs, firstlotAttribs);
            }
        }
    }
}