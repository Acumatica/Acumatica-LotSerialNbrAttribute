[![Project Status](http://opensource.box.com/badges/active.svg)](http://opensource.box.com/badges)

Adding Attribute Support to Lot/Serial Number
==================================
Acumatica ERP lets you define attributes for flexible, meaningful classification of an Entity (Lead, Stock/Non-Stock Items Etc.) as required for your company's specific needs. An attribute is a property that enables you to specify additional information for Entity in the system. 

This extension allows to add attribute support to Lot/Serial Number so that each Lot/Serial Number's unique characteristic (Example – color variation, stone pattern etc. in case of Granite Slab) can be tracked. With this extension, you can:

* Define attributes to be tracked for each Lot/Serial class
* Assign attribute values while receiving items via Allocations dialog in Purchase Receipt and Inventory Receipt screens
* Assign attribute values and image for individual Lot/Serial Number
* Bulk assignment of attribute values via Import Scenario 
* Search and allocate Lot/Serial Number by attribute value/s in Sales Order screen

### Prerequisites
* Acumatica 2017 R2 (17.209.0028+) or higher
* Acumatica 2018 R1 (18.102.0048+) or higher

Quick Start
-----------

### Installation

##### Install customization deployment package
1. Download PXLotSertialNbrAttributeExtPkg.zip from this repository
2. In your Acumatica ERP instance, navigate to System -> Customization -> Customization Projects (SM204505), import PXLotSertialNbrAttributeExtPkg.zip as a customization project
3. Publish customization project.

### Usage

1. Go to Attributes Screen (CS205000) and create new attributes if you need to.
2. Navigate to Lot/Serial classes Screen (IN207000) and select the class for which you need to specify list of Attributes.
![Screenshot](/_ReadMeImages/IN207000.png)

3. Navigate to Stock Item Screen (IN202500) and create a new Stock Item having Lot/Serial class created in Step # 2.
4. Create Purchase Order (PO301000) for Stock Item created in Step # 3. And move forward with creating Purchase Receipt (PO302000) for this Purchase Order.
5. Click on Allocations button, you should be able to see Attributes specified in Step # 2 and can specify value for them.
![Screenshot](/_ReadMeImages/PO302000Allocation.png)

6. **Apply Attribute from First** button copies attribute values specified for very first Lot/Serial Number and applies to rest of the Lot/Serial Number displayed in the dialog. One can specify value for each individual Lot/Serial Number as well.
7. Attribute value can be assigned for Lot/Serial Number while receiving inventory via IN Receipt Screen (IN301000) similarly via Allocations dialog.
8. Attribute values and image can be assigned to Lot/Serial Number/s via custom **Item Lot/Serial # Info** screen (IN202501) as well.
![Screenshot](/_ReadMeImages/IN202501.png)

9. Attribute values can be imported via **Import Lot/Serial Attributes** custom import scenario available via customization package.
![Screenshot](/_ReadMeImages/SM206025.png)
![Screenshot](/_ReadMeImages/SM206036.png)

10. **Item Lot/Serial Search** option can be utilized to search Lot/Serial Number by attribute value and allocate in Sales Order Entry screen (SO301000). Columns for Attribute/s will be dynamically added/removed based on specified Lot/Serial class value.
![Screenshot](/_ReadMeImages/SO301000-1.png)
![Screenshot](/_ReadMeImages/SO301000-2.png)

Known Issues
------------
None at the moment

## Copyright and License

Copyright © `2018` `Acumatica`

This component is licensed under the MIT License, a copy of which is available online [here](LICENSE.md)
