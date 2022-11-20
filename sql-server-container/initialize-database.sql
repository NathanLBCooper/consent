IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'consentdb')
  BEGIN
    CREATE DATABASE consentdb;
  END
