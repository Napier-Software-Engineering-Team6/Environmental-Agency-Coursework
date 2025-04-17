# Environmental-Agency-Coursework

SET09102 2024-5 TR2 001 - Software Engineering

# Database Setup

create appsettings.json file in CourseworkApp/Database/

Below commands in db

on master
CREATE LOGIN notesapp WITH PASSWORD='N0tesApp$';
CREATE user notesapp for login notesapp;
grant create any database to notesapp;

on testdb
CREATE user notesapp for login notesapp testdb
GRANT control on DATABASE::testdb to notesapp;
