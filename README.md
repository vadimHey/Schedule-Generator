# ScheduleGenerator

## �������� �������

���-���������� �� ASP.NET Core Razor Pages ��� ����������� ���������� ������� � ��� �����������. 
��� ������������� �������� �������� CRUD � ������������� ��������� �������� ������.

---

## ������ ������

1. �������� � ���������� PostgreSQL � ������������ �����: https://www.postgresql.org/download/

2. ��������� ����������� � ����. � ����� `appsettings.json` �������� �������� Password �� ���� ������ �� ������� ��:
    
    ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=ScheduleDb;Username=postgres;Password=YOUR_PASSWORD"
     }
   }
