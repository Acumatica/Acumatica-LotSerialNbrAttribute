using System;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.IN;

namespace PX.LotSertialNbrAttribute.Ext
{
    [Serializable]
    public class UsrInventoryLotSerialContainer : IBqlTable
    {
        #region ContainerID
        public abstract class containerID : IBqlField { }

        protected int? _ContainerID;
        [PXDBIdentity()]
        public virtual int? ContainerID
        {
            get
            {
                return this._ContainerID;
            }
            set
            {
                this._ContainerID = value;
            }
        }
        #endregion
        #region InventoryID
        public abstract class inventoryID : IBqlField { }

        protected int? _InventoryID;
        [Inventory(IsKey = true)]
        [PXDefault]
        [PXUIField(DisplayName = "Inventory ID")]
        public virtual int? InventoryID
        {
            get
            {
                return this._InventoryID;
            }
            set
            {
                this._InventoryID = value;
            }
        }
        #endregion
        #region LotSerialNbr
        public abstract class lotSerialNbr : IBqlField { }

        protected string _LotSerialNbr;
        [PXDBString(100, IsUnicode = true, IsKey = true, InputMask = "")]
        [PXSelector(typeof(Search<INItemLotSerial.lotSerialNbr,
                                    Where<INItemLotSerial.inventoryID, 
                                        Equal<Optional<UsrInventoryLotSerialContainer.inventoryID>>>>),
                           typeof(INItemLotSerial.lotSerialNbr))]
        [PXDefault]
        [PXUIField(DisplayName = "Lot/Serial Nbr")]
        public virtual string LotSerialNbr
        {
            get
            {
                return this._LotSerialNbr;
            }
            set
            {
                this._LotSerialNbr = value;
            }
        }
        #endregion

        #region LotSerClassID
        public abstract class lotSerClassID : IBqlField { }

        protected String _LotSerClassID;
        [PXDBString(10, IsUnicode = true)]
        [PXUIField(DisplayName = "Lot/Serial Class", IsReadOnly = true, Enabled = false)]
        [PXSelector(typeof(Search<INLotSerClass.lotSerClassID>))]
        [PXDefault(typeof(Search<InventoryItem.lotSerClassID,
                                    Where<InventoryItem.inventoryID, Equal<Current<UsrInventoryLotSerialContainer.inventoryID>>>>),
                                    PersistingCheck = PXPersistingCheck.Nothing)]
        [PXFormula(typeof(Default<UsrInventoryLotSerialContainer.inventoryID>))]
        public virtual String LotSerClassID
        {
            get
            {
                return this._LotSerClassID;
            }
            set
            {
                this._LotSerClassID = value;
            }
        }
        #endregion

        #region ImageUrl
        public abstract class imageUrl : IBqlField { }

        protected String _ImageUrl;

        [PXDBString(255)]
        public virtual String ImageUrl
        {
            get
            {
                return this._ImageUrl;
            }
            set
            {
                this._ImageUrl = value;
            }
        }
        #endregion

        #region NoteID
        public abstract class noteID : IBqlField { }

        protected Guid? _NoteID;
        [PXNote]
        public virtual Guid? NoteID
        {
            get
            {
                return this._NoteID;
            }
            set
            {
                this._NoteID = value;
            }
        }
        #endregion

        #region tstamp
        public abstract class Tstamp : IBqlField { }

        protected Byte[] _tstamp;
        [PXDBTimestamp()]
        public virtual Byte[] tstamp
        {
            get
            {
                return this._tstamp;
            }
            set
            {
                this._tstamp = value;
            }
        }
        #endregion
        #region CreatedByID
        public abstract class createdByID : IBqlField { }

        protected Guid? _CreatedByID;
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID
        {
            get
            {
                return this._CreatedByID;
            }
            set
            {
                this._CreatedByID = value;
            }
        }
        #endregion
        #region CreatedByScreenID
        public abstract class createdByScreenID : IBqlField { }

        protected string _CreatedByScreenID;
        [PXDBCreatedByScreenID()]
        public virtual string CreatedByScreenID
        {
            get
            {
                return this._CreatedByScreenID;
            }
            set
            {
                this._CreatedByScreenID = value;
            }
        }
        #endregion
        #region CreatedDateTime
        public abstract class createdDateTime : IBqlField { }

        protected DateTime? _CreatedDateTime;
        [PXDBCreatedDateTime()]
        public virtual DateTime? CreatedDateTime
        {
            get
            {
                return this._CreatedDateTime;
            }
            set
            {
                this._CreatedDateTime = value;
            }
        }
        #endregion
        #region LastModifiedByID
        public abstract class lastModifiedByID : IBqlField { }

        protected Guid? _LastModifiedByID;
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID
        {
            get
            {
                return this._LastModifiedByID;
            }
            set
            {
                this._LastModifiedByID = value;
            }
        }
        #endregion
        #region LastModifiedByScreenID
        public abstract class lastModifiedByScreenID : IBqlField { }

        protected string _LastModifiedByScreenID;
        [PXDBLastModifiedByScreenID()]
        public virtual string LastModifiedByScreenID
        {
            get
            {
                return this._LastModifiedByScreenID;
            }
            set
            {
                this._LastModifiedByScreenID = value;
            }
        }
        #endregion
        #region LastModifiedDateTime
        public abstract class lastModifiedDateTime : IBqlField { }

        protected DateTime? _LastModifiedDateTime;
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime
        {
            get
            {
                return this._LastModifiedDateTime;
            }
            set
            {
                this._LastModifiedDateTime = value;
            }
        }
        #endregion

        public abstract class attributes : IBqlField { }

        [CRAttributesField(typeof(UsrInventoryLotSerialContainer.lotSerClassID))]
        public virtual string[] Attributes { get; set; }

        //For SmartPanel
        public abstract class lotSerialAttributes : IBqlField { }

        [PXAddAtttributeColumns(typeof(ItemLotSerialAdvSearchFilter),
                "ItemLotSerialClass",
                typeof(UsrInventoryLotSerialContainer.lotSerClassID),
                typeof(UsrInventoryLotSerialContainer.noteID), false)]
        public virtual string[] LotSerialAttributes { get; set; }
    }
}