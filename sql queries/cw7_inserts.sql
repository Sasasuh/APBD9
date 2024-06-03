
INSERT INTO Client (FirstName,LastName,Email,Telephone,Pesel)
VALUES
  ('John','Smith','dolor.tempus.non@protonmail.couk','(0766) 52574466',32080718531),
  ('Jake','Doe','et.pede.nunc@yahoo.edu','(01334) 4841396',52121471467),
  ('Isadora','Clarke','eget.tincidunt.dui@aol.ca','(034568) 645965',92110685522),
  ('Hilel','Meyers','tempus.scelerisque@hotmail.com','(0654) 92477496',50110114541),
  ('Orla','West','nunc.quisque@hotmail.couk','(0663) 16138381',48050201844),
  ('Chandler','Chavez','magna.praesent@yahoo.couk','(04851) 2449635',78080785107),
  ('Conan','Christensen','consequat.dolor.vitae@google.couk','(034824) 376192',87010737651),
  ('Jane','Landry','molestie.orci.tincidunt@aol.org','(06631) 1921542',88112655578),
  ('Gloria','Potts','donec.dignissim@protonmail.org','(037994) 352565',23050354026),
  ('Colorado','Norman','cursus@protonmail.edu','(0543) 62363536',66080382706);


INSERT INTO Country (Name)
VALUES
  ('Spain'),
  ('Poland'),
  ('Germany'),
  ('France'),
  ('Austria'),
  ('Singapore'),
  ('Czech republic'),
  ('Singapore'),
  ('Italy'),
  ('Russian Federation');
  
INSERT INTO Trip (Name,Description,DateFrom,DateTo,MaxPeople)
VALUES 
  ('ABC', 'Lorem ipsum...', '2023-04-01', '2023-04-10', 2),
  ('DEF', 'Lorem...', '2023-03-01', '2023-02-10', 4);
  
INSERT INTO Country_Trip (IdCountry, IdTrip)
VALUES
  (2, 1),
  (3, 1),
  (1, 2),
  (4, 2),
  (7, 2);
  
INSERT INTO Client_Trip (IdClient,IdTrip,RegisteredAt,PaymentDate)
VALUES
  (1, 1, '2023-03-22', '2023-03-24'),
  (2, 1, '2023-03-24', '2023-02-26'),
  (3, 2, '2023-03-24', '2023-02-26'),
  (4, 2, '2023-03-24', '2023-02-26'),
  (5, 2, '2023-03-24', '2023-02-26');