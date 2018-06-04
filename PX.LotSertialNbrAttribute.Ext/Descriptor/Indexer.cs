using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.CS;

namespace PX.LotSertialNbrAttribute.Ext
{
    public class CRAttributeListPXLotSerExt<TEntity, TParentEntity, TParentField> : CRAttributeList<TEntity>
        where TEntity : class, IBqlTable, new()
        where TParentEntity : class, IBqlTable, new()
        where TParentField : IBqlField
    {
        public CRAttributeListPXLotSerExt(PXGraph graph) : base(graph)
        {
        }

        protected override IEnumerable SelectDelegate()
        {
            if (_Graph.Caches[typeof(TParentEntity)].Current != null)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(_Graph.Caches[typeof(TParentEntity)].GetValue(_Graph.Caches[typeof(TParentEntity)].Current,
                                                                  typeof(TParentField).Name))))
                {
                    _Graph.Caches[typeof(TEntity)].Current = _Graph.Views["InventoryLotSerialContainer"].SelectSingle();
                    if (_Graph.Caches[typeof(TEntity)].Current != null)
                    {
                        PXParentAttribute.SetParent(_Graph.Caches[typeof(TEntity)], _Graph.Caches[typeof(TEntity)].Current,
                                                                  typeof(TParentEntity), _Graph.Caches[typeof(TParentEntity)].Current);
                    }
                }
                else
                    _Graph.Caches[typeof(TEntity)].Current = null;
            }
            else
                _Graph.Caches[typeof(TEntity)].Current = null;
            return base.SelectDelegate();
        }
    }

    public class PXAddAtttributeColumns : CRAttributesFieldAttribute
    {
        private Type _ClassInfoRow;
        private string _ClassInfoFieldName;
        private string _StrLotSerialClassAttributes;
        public PXAddAtttributeColumns(Type ClassInfoRow, string ClassInfoFieldName, Type classIDField, Type noteIdField,
                                      bool IsForSelector = true)
            : base(classIDField, noteIdField)
        {
            _ClassInfoRow = ClassInfoRow;
            _ClassInfoFieldName = ClassInfoFieldName;
        }

        public override void CacheAttached(PXCache sender)
        {
            if (_StrLotSerialClassAttributes != null)
            {
                sender.Fields.RemoveAll(x => x.Contains("_LotSerialAttributes"));
            }

            this._IsActive = true;
            _StrLotSerialClassAttributes = String.Empty;
            var classInfoRow = sender.Graph.Caches[_ClassInfoRow].Current;
            if (classInfoRow != null)
            {
                string ClassID = Convert.ToString(sender.Graph.Caches[_ClassInfoRow].GetValue(classInfoRow, _ClassInfoFieldName));
                PXTrace.WriteInformation(ClassID);
                List<string> fldList = sender.Fields.Where(x => x.Contains("_LotSerialAttributes")).ToList();
                if (!String.IsNullOrEmpty(ClassID))
                {
                    List<CSAttributeGroup> attribList = PXSelectReadonly<CSAttributeGroup, Where<CSAttributeGroup.entityType, Equal<Required<CSAttributeGroup.entityType>>,
                                                            And<CSAttributeGroup.entityClassID, Equal<Required<CSAttributeGroup.entityClassID>>>>>
                                                            .Select(sender.Graph, typeof(UsrInventoryLotSerialContainer).FullName,
                                                                          ClassID).RowCast<CSAttributeGroup>().ToList();
                    foreach (var attrib in attribList)
                    {
                        if (String.IsNullOrEmpty(_StrLotSerialClassAttributes))
                            _StrLotSerialClassAttributes = attrib.AttributeID;
                        else
                            _StrLotSerialClassAttributes = _StrLotSerialClassAttributes + "," + attrib.AttributeID;
                    }
                }
            }
            base.CacheAttached(sender);
        }

        protected override void AttributeFieldSelecting(PXCache sender, PXFieldSelectingEventArgs e,
                                                        PXFieldState state, string attributeName, int idx)
        {
            //Out-of-box DisplayName is prefixed with "$Attributes$-" - if you need to take that off.
            state.DisplayName = (!String.IsNullOrEmpty(state.DisplayName)) ? (state.DisplayName.Replace("$Attributes$-", "")) : attributeName;
            if (_StrLotSerialClassAttributes == null || !_StrLotSerialClassAttributes.Contains(state.Name.Replace("_LotSerialAttributes", "")))
            {
                state.Visible = false;
                state.Visibility = PXUIVisibility.Invisible;
            }
            else
            {
                state.Visible = true;
                //Requires AutoGenerateColumns="AppendDynamic" for PXGrid Control for dynamic Attribute columns creation
                state.Visibility = PXUIVisibility.Dynamic;
            }
            e.IsAltered = true;
            base.AttributeFieldSelecting(sender, e, state, attributeName, idx);
        }
    }
}