using System;
using System.Collections.Generic;
using System.Linq;
using PX.Data;
using PX.Objects.IN;

namespace PX.LotSertialNbrAttribute.Ext
{
    public class INReceiptEntryPXExt : PXGraphExtension<INReceiptEntry>
    {
        [PXCopyPasteHiddenView]
        public PXSelect<UsrInventoryLotSerialContainer, 
               Where<UsrInventoryLotSerialContainer.inventoryID, Equal<Current<INTranSplit.inventoryID>>,
               And<UsrInventoryLotSerialContainer.lotSerialNbr, Equal<Current<INTranSplit.lotSerialNbr>>>>> InventoryLotSerialContainer;

        [PXCopyPasteHiddenView]
        public CRAttributeListPXLotSerExt<UsrInventoryLotSerialContainer, INTranSplit, INTranSplit.lotSerialNbr> Attributes;

        public void INTranSplit_RowInserted(PXCache sender, PXRowInsertedEventArgs e, PXRowInserted BaseInvoke)
        {
            if (BaseInvoke != null)
                BaseInvoke(sender, e);

            AssociateAttributeToLotSerial((INTranSplit)e.Row);
        }

        public void INTranSplit_RowUpdated(PXCache sender, PXRowUpdatedEventArgs e, PXRowUpdated BaseInvoke)
        {
            if (BaseInvoke != null)
                BaseInvoke(sender, e);

            AssociateAttributeToLotSerial((INTranSplit)e.Row);
        }

        public void AssociateAttributeToLotSerial(INTranSplit line)
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

        public void INTran_RowSelected(PXCache sender, PXRowSelectedEventArgs e, PXRowSelected BaseInvoke)
        {
            if (BaseInvoke != null)
                BaseInvoke(sender, e);

            if (e.Row == null) return;

            INTranPXExt lineExt = PXCache<INTran>.GetExtension<INTranPXExt>((INTran)e.Row);
            ApplyAttributes.SetVisible(lineExt.UsrLineLotSerTrack != INLotSerTrack.NotNumbered);
        }

        public PXAction<INRegister> ApplyAttributes;
        [PXButton]
        [PXUIField(DisplayName = "Apply Attribute From First", MapEnableRights = PXCacheRights.Update, MapViewRights = PXCacheRights.Update)]
        public virtual void applyAttributes()
        {
            List<INTranSplit> splitList = Base.splits.Select().RowCast<INTranSplit>().Select(PXCache<INTranSplit>.CreateCopy).ToList();
            if (splitList.Count <= 1) return;
            INTranSplit firstSplit = splitList.First();
            UsrInventoryLotSerialContainer firstlotAttribs = PXSelect<UsrInventoryLotSerialContainer,
                                                                Where<UsrInventoryLotSerialContainer.inventoryID, Equal<Required<INTranSplit.inventoryID>>,
                                                                    And<UsrInventoryLotSerialContainer.lotSerialNbr, Equal<Required<INTranSplit.lotSerialNbr>>>>>
                                                                    .Select(Base, firstSplit.InventoryID, firstSplit.LotSerialNbr);
            foreach (INTranSplit split in splitList)
            {
                UsrInventoryLotSerialContainer lotAttribs = PXSelect<UsrInventoryLotSerialContainer,
                                                                    Where<UsrInventoryLotSerialContainer.inventoryID, Equal<Required<INTranSplit.inventoryID>>,
                                                                        And<UsrInventoryLotSerialContainer.lotSerialNbr, Equal<Required<INTranSplit.lotSerialNbr>>>>>
                                                                        .Select(Base, split.InventoryID, split.LotSerialNbr);
                if (firstlotAttribs.NoteID != lotAttribs.NoteID)
                    Attributes.CopyAllAttributes(lotAttribs, firstlotAttribs);
            }
        }
    }
}