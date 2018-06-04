using System;
using PX.Data;
using PX.Objects.IN;

namespace PX.LotSertialNbrAttribute.Ext
{
    public class INTranPXExt : PXCacheExtension<INTran>
    {
        #region UsrLineLotSerTrack

        public abstract class usrLineLotSerTrack : IBqlField { }

        [PXString(1)]
        [PXDefault(typeof(Search2<INLotSerClass.lotSerTrack,
                                    InnerJoin<InventoryItem, On<InventoryItem.lotSerClassID, Equal<INLotSerClass.lotSerClassID>>>,
                                    Where<InventoryItem.inventoryID, Equal<Current<INTran.inventoryID>>>>),
                                    PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<INTran.inventoryID>))]
        [PXUIField(DisplayName = "Is Serial Numbered", Visibility = PXUIVisibility.Invisible, Visible = false, Enabled = false)]
        [PXDBScalar(typeof(Search2<INLotSerClass.lotSerTrack,
                                    InnerJoin<InventoryItem, On<InventoryItem.lotSerClassID, Equal<INLotSerClass.lotSerClassID>>>,
                                    Where<InventoryItem.inventoryID, Equal<INTran.inventoryID>>>))]
        public virtual String UsrLineLotSerTrack { get; set; }

        #endregion
    }
}
