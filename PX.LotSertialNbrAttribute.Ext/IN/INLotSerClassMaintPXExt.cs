using PX.Data;
using PX.Objects.IN;
using PX.Objects.CR;
using PX.Objects.CS;
using System.Collections;

namespace PX.LotSertialNbrAttribute.Ext
{
    public class INLotSerClassMaintPXExt : PXGraphExtension<INLotSerClassMaint>
    {
        public CSAttributeGroupList<INLotSerClass, UsrInventoryLotSerialContainer> Mapping;

        public PXAction<INLotSerClass> showDetails;
        [PXUIField(DisplayName = "Details", MapEnableRights = PXCacheRights.Select, MapViewRights = PXCacheRights.Select)]
        [PXLookupButton]
        public virtual IEnumerable ShowDetails(PXAdapter adapter)
        {
            CSAttribute CurrentAttibute = PXSelect<CSAttribute,
                Where<CSAttribute.attributeID, Equal<Current<CSAttributeGroup.attributeID>>>>.Select(Base);

            if (CurrentAttibute != null)
            {
                CSAttributeMaint graph = PXGraph.CreateInstance<CSAttributeMaint>();
                graph.Clear();
                graph.Attributes.Current = CurrentAttibute;
                throw new PXRedirectRequiredException(graph, "INLotSerClassMaintPXExt");
            }

            return adapter.Get();
        }
    }
}
