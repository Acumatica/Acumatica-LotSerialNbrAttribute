using System;
using PX.Data;
using PX.Objects.IN;
using System.Collections.Generic;
using PX.Objects.CS;

namespace PX.LotSertialNbrAttribute.Ext
{
    [Serializable]
    public class ItemLotSerialAdvSearchFilter : IBqlTable
    {
        #region SearchString

        public abstract class searchString : IBqlField { }

        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = "Search String")]
        public virtual string SearchString { get; set; }

        #endregion

        #region ItemLotSerialClass

        public abstract class itemLotSerialClass : IBqlField { }

        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "Lot/Serial Class")]
        [PXSelector(typeof(INLotSerClass.lotSerClassID),
                            new Type[] 
                                {
                                    typeof(INLotSerClass.lotSerClassID),
                                    typeof(INLotSerClass.descr)
                                }
        )]
        [PXDefault]
        public virtual string ItemLotSerialClass { get; set; }

        #endregion

        #region OnlyAvailable

        public abstract class onlyAvailable : IBqlField { }

        [PXDBBool]
        [PXDefault(true)]
        [PXUIField(DisplayName = "Show Available Items Only")]
        public virtual bool? OnlyAvailable { get; set; }

        #endregion
    }
}