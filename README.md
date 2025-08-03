SHIPMENT COMPANY

Application ShipmentSolution created is covering all needs for one Shipment Company including all chains of working process.
I/ It consists of:
1/ Database named Shipments which have 5 entities with relationships as showed on the diagram below:
 <img width="490" height="333" alt="image" src="https://github.com/user-attachments/assets/10423f69-a95e-4b14-a3c1-08cb3c5def22" />


  	2/ WEB application has 6 screens(Views) : Main , Shipments, Customers, Deliveries, Mail Carriers, Routes. It can be chosen appropriate screen using Upper menu or Down menu on screen. Shipments, Customers, Deliveries, Mail Carriers and Routes have in addition 3 screens - Edit,Delete,Create. Totally we have 16 screens and in addition we have for Login + Register users.

II/ For this Application we have 3 types of Users: Admin, Loged User and Unlogged User.
Admin User can do anything.
Loged user can list, edit and create own records for Shipments, Customers, Deliveries, Mail Carriers and Routes
Unlogged User can't list,edit,create,delete records. In case user is not loged in then it appeared message requested him to log in.: <img width="944" height="422" alt="image" src="https://github.com/user-attachments/assets/554f31e8-8f3f-4b32-b4fb-45a0f764432c" />


III/ Main Screen :
 <img width="936" height="420" alt="image" src="https://github.com/user-attachments/assets/84b926c6-5536-45bb-a72e-33aa6f44e6ad" />


It has Upper menu where each screen can be selected and also Logout, Login or Register options. This Upper Menu can be seen on all other screens as well.
Under pictures we have buttons through which user can see all 5 screens

IV/ Shipments :
<img width="935" height="422" alt="image" src="https://github.com/user-attachments/assets/6e8d8013-baf6-4418-8ec9-198b047f4c2f" />
 
Here we have options to search records by customer name or by shipping method. Also it has a possibility to Reset the searching text .
User can see the table with listing of all Shipments and depending of the role the User can Create (button Create Shipment), Edit (button Edit) and Delete (button Delete) the records.
Create View: <img width="845" height="410" alt="image" src="https://github.com/user-attachments/assets/20f1eed4-1f76-4a9f-9714-3e27a9bdc75c" />  Here it is appear empty form and user need to fill it and to click button Save or button Cancel in case he want to back without creating new shipment. After clicking one of both buttons it backs to main screen for Shimpent (list of Shipment)
Edit View: <img width="948" height="399" alt="image" src="https://github.com/user-attachments/assets/ebfbe147-3f40-437f-8135-054410890284" />   After clicking on Edit button it appears Edit View. Here the user can edit some field and then to click Save or Cancel buttons (functuonality of those 2 buttons is same as per Create View mentioned above)
Delete View: <img width="950" height="407" alt="image" src="https://github.com/user-attachments/assets/1f51c658-10db-4f75-9ebc-9c950cb17ed9" /> After clicking delete button it appears Delete View and user have 2 buttons again: Delete and Cancel. On clicking Delete then the record is deleted (soft delete). On clicking button cancel then process of deleting is cancelled and on after clicking of both buttons it backs to listing of Shipments




V/ Customers:
 <img width="680" height="281" alt="image" src="https://github.com/user-attachments/assets/e691aa00-d573-4ecd-809f-7f60cd239426" />


Here we have options to search records by First or Last name of Customer. Also it has a possibility to Reset the searching text.
User can see the table with listing of all Shipments and depending of the role the User can Create (button Create Shipment), Edit (button Edit) and Delete (button Delete) the records.
VI/ Deliveries:
 <img width="688" height="329" alt="image" src="https://github.com/user-attachments/assets/1f9f5540-fc29-4807-b4fe-28452d37bb5a" />


Here we have options to search records by Carrier name or filter by Carrier. Also it has a possibility to Reset the searching text.
User can see the table with listing of all Shipments and depending of the role the User can Create (button Create Shipment), Edit (button Edit) and Delete (button Delete) the records.

VII/ Mail Carriers:
<img width="694" height="331" alt="image" src="https://github.com/user-attachments/assets/2ce17a01-d7ad-4cad-8fb3-1f7f5d7a332d" />

 
Here we have options to search records by Mail Carrier name or filter by Statuses. Also it has a possibility to Reset the searching text.
User can see the table with listing of all Shipments and depending of the role the User can Create (button Create Shipment), Edit (button Edit) and Delete (button Delete) the records.

VIII/ Routes:
<img width="731" height="332" alt="image" src="https://github.com/user-attachments/assets/6a847b3e-25ee-4594-a2d8-b3a1133228db" />

 
Here we have options to search records Start or End of Routes or filter Priorities. Also it has a possibility to Reset the searching text.
User can see the table with listing of all Shipments and depending of the role the User can Create (button Create Shipment), Edit (button Edit) and Delete (button Delete) the records.







