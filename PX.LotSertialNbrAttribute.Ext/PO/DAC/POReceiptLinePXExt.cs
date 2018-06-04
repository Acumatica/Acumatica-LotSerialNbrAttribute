using System;
using PX.Data;
using PX.Objects.IN;
using PX.Objects.PO;

namespace PX.LotSertialNbrAttribute.Ext
{
    public class POReceiptLinePXExt : PXCacheExtension<POReceiptLine>
    {
        #region UsrLineLotSerTrack

        public abstract class usrLineLotSerTrack : IBqlField { }

        [PXString(1)]
        [PXDefault(typeof(Search2<INLotSerClass.lotSerTrack,
                                    InnerJoin<InventoryItem, On<InventoryItem.lotSerClassID, Equal<INLotSerClass.lotSerClassID>>>,
                                    Where<InventoryItem.inventoryID, Equal<Current<POReceiptLine.inventoryID>>>>),
                                    PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<POReceiptLine.inventoryID>))]
        [PXUIField(DisplayName = "Is Serial Numbered", Visibility = PXUIVisibility.Invisible, Visible = false, Enabled = false)]
        [PXDBScalar(typeof(Search2<INLotSerClass.lotSerTrack,
                                    InnerJoin<InventoryItem, On<InventoryItem.lotSerClassID, Equal<INLotSerClass.lotSerClassID>>>,
                                    Where<InventoryItem.inventoryID, Equal<POReceiptLine.inventoryID>>>))]
        public virtual String UsrLineLotSerTrack { get; set; }

        #endregion
    }
}
