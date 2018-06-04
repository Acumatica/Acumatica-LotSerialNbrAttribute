using PX.Data;
using PX.Objects.CR;

namespace PX.LotSertialNbrAttribute.Ext
{
    public class InventoryLotSerialContainerMaint : PXGraph<InventoryLotSerialContainerMaint, UsrInventoryLotSerialContainer>
    {
        public PXSelect<UsrInventoryLotSerialContainer> InventoryLotSerialContainers;

        public PXSelect<UsrInventoryLotSerialContainer,
                    Where<UsrInventoryLotSerialContainer.inventoryID, Equal<Optional<UsrInventoryLotSerialContainer.inventoryID>>,
                    And<UsrInventoryLotSerialContainer.lotSerialNbr, Equal<Optional<UsrInventoryLotSerialContainer.lotSerialNbr>>>>> InventoryLotSerialContainerSettings;

        public CRAttributeList<UsrInventoryLotSerialContainer> Attributes;
    }
}