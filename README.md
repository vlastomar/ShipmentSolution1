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
User can see the table with listing of all Shipments and depending of the role the User can Create (button Create Shipment), Edit (button Edit) and Delete (button Delete) the records. It has and button Back which on pressing will return to the Main screen.
Create View: <img width="845" height="410" alt="image" src="https://github.com/user-attachments/assets/20f1eed4-1f76-4a9f-9714-3e27a9bdc75c" />  Here it is appear empty form and user need to fill it and to click button Save or button Cancel in case he want to back without creating new shipment. After clicking one of both buttons it backs to main screen for Shimpent (list of Shipment)
Edit View: <img width="948" height="399" alt="image" src="https://github.com/user-attachments/assets/ebfbe147-3f40-437f-8135-054410890284" />   After clicking on Edit button it appears Edit View. Here the user can edit some field and then to click Save or Cancel buttons (functuonality of those 2 buttons is same as per Create View mentioned above)
Delete View: <img width="950" height="407" alt="image" src="https://github.com/user-attachments/assets/1f51c658-10db-4f75-9ebc-9c950cb17ed9" /> After clicking delete button it appears Delete View and user have 2 buttons again: Delete and Cancel. On clicking Delete then the record is deleted (soft delete). On clicking button cancel then process of deleting is cancelled and on after clicking of both buttons it backs to listing of Shipments




V/ Customers:
 <img width="949" height="422" alt="image" src="https://github.com/user-attachments/assets/65e11bca-7bed-484b-844b-6ddeb7b47925" />

Here we have options to search records by First or Last name of Customer. Also it has a possibility to Reset the searching text.
User can see the table with listing of all Shipments and depending of the role the User can Create (button Create Shipment), Edit (button Edit) and Delete (button Delete) the records.It has and button Back which on pressing will return to the Main screen.
Create View: <img width="945" height="422" alt="image" src="https://github.com/user-attachments/assets/7ae476ac-9c88-4824-bd2f-dcc944febb73" />
  Here it is appear empty form and user need to fill it and to click button Save or button Cancel in case he want to back without creating new customer. After clicking one of both buttons it backs to main screen for Customer (list of Customer)
Edit View: <img width="949" height="424" alt="image" src="https://github.com/user-attachments/assets/d0c7f551-52b4-4981-b4ec-68cc2a9844de" />
   After clicking on Edit button it appears Edit View. Here the user can edit some field and then to click Save or Cancel buttons (functuonality of those 2 buttons is same as per Create View mentioned above)
Delete View: <img width="943" height="307" alt="image" src="https://github.com/user-attachments/assets/9f8ab14a-9ede-4654-a6b0-df99912f7ff5" />
 After clicking delete button it appears Delete View and user have 2 buttons again: Delete and Cancel. On clicking Delete then the record is deleted (soft delete). On clicking button cancel then process of deleting is cancelled and on after clicking of both buttons it backs to listing of Customer

VI/ Deliveries:
 <img width="941" height="422" alt="image" src="https://github.com/user-attachments/assets/834344f1-ffee-4b9b-94f2-c910b8588760" />

Here we have options to search records by Carrier name or filter by Carrier. Also it has a possibility to Reset the searching text.
User can see the table with listing of all Shipments and depending of the role the User can Create (button Create Shipment), Edit (button Edit) and Delete (button Delete) the records.It has and button Back which on pressing will return to the Main screen.
Create View: <img width="950" height="422" alt="image" src="https://github.com/user-attachments/assets/fe5b3325-0aed-454e-baa4-44a4dcf066e9" />
Here it is appear empty form and user need to fill it and to click button Save or button Cancel in case he want to back without creating new delivery. After clicking one of both buttons it backs to main screen for Delivery (list of Delivery)
Edit View: <img width="943" height="389" alt="image" src="https://github.com/user-attachments/assets/8beebb74-eeac-4f4d-90dd-3972ef3f21d9" />
After clicking on Edit button it appears Edit View. Here the user can edit some field and then to click Save or Cancel buttons (functuonality of those 2 buttons is same as per Create View mentioned above)
Delete View: <img width="947" height="274" alt="image" src="https://github.com/user-attachments/assets/d6d43c06-1fd5-431c-9212-97d3a59ae7cc" />
After clicking delete button it appears Delete View and user have 2 buttons again: Delete and Cancel. On clicking Delete then the record is deleted (soft delete). On clicking button cancel then process of deleting is cancelled and on after clicking of both buttons it backs to listing of Delivery

 
VII/ Mail Carriers:
<img width="947" height="389" alt="image" src="https://github.com/user-attachments/assets/f8d1eb07-f474-4f5c-b687-7d3124633048" />

Here we have options to search records by Mail Carrier name or filter by Statuses. Also it has a possibility to Reset the searching text.
User can see the table with listing of all Shipments and depending of the role the User can Create (button Create Shipment), Edit (button Edit) and Delete (button Delete) the records.It has and button Back which on pressing will return to the Main screen.
Create View: <img width="941" height="353" alt="image" src="https://github.com/user-attachments/assets/cf602394-0546-4c46-990e-a1c5c1e64a18" />
Here it is appear empty form and user need to fill it and to click button Save or button Cancel in case he want to back without creating new mail carrier. After clicking one of both buttons it backs to main screen for Mail Carrier (list of Mail Carrier)
Edit View: <img width="941" height="377" alt="image" src="https://github.com/user-attachments/assets/543f3340-0f69-4257-a591-e2b8e44f4a1a" />
After clicking on Edit button it appears Edit View. Here the user can edit some field and then to click Save or Cancel buttons (functuonality of those 2 buttons is same as per Create View mentioned above)
Delete View: <img width="947" height="393" alt="image" src="https://github.com/user-attachments/assets/9413e687-1bee-4122-9f73-6c7167e5dea0" />
After clicking delete button it appears Delete View and user have 2 buttons again: Delete and Cancel. On clicking Delete then the record is deleted (soft delete). On clicking button cancel then process of deleting is cancelled and on after clicking of both buttons it backs to listing of Mail Carrier


VIII/ Routes:
<img width="946" height="403" alt="image" src="https://github.com/user-attachments/assets/3e9ed083-a0f8-40d8-9193-4f55605c43f9" />
Here we have options to search records Start or End of Routes or filter Priorities. Also it has a possibility to Reset the searching text.
User can see the table with listing of all Shipments and depending of the role the User can Create (button Create Shipment), Edit (button Edit) and Delete (button Delete) the records.It has and button Back which on pressing will return to the Main screen.

Create View: <img width="946" height="386" alt="image" src="https://github.com/user-attachments/assets/637f3bd1-6c90-4c05-9263-8603cfab05be" />
Here it is appear empty form and user need to fill it and to click button Save or button Cancel in case he want to back without creating new mail route. After clicking one of both buttons it backs to main screen for Route (list of Route)
Edit View: <img width="949" height="334" alt="image" src="https://github.com/user-attachments/assets/a459c7e2-f585-4f6b-9044-1d8ec59222c2" />
After clicking on Edit button it appears Edit View. Here the user can edit some field and then to click Save or Cancel buttons (functuonality of those 2 buttons is same as per Create View mentioned above)
Delete View: <img width="947" height="364" alt="image" src="https://github.com/user-attachments/assets/eba1248f-b196-4500-8dbf-3c58077e3982" />
After clicking delete button it appears Delete View and user have 2 buttons again: Delete and Cancel. On clicking Delete then the record is deleted (soft delete). On clicking button cancel then process of deleting is cancelled and on after clicking of both buttons it backs to listing of Route







