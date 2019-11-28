CREATE TABLE  IF NOT EXISTS employee (
employee_id BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
  name VARCHAR(30) NOT NULL,
  last_name1 VARCHAR(30) NOT NULL,
  last_name2 VARCHAR(30) NULL,
  address VARCHAR(30) NOT NULL,
  telephone VARCHAR(30) NULL,
  role_id BIGINT UNSIGNED NOT NULL,
  store_id BIGINT UNSIGNED NOT NULL,
  username VARCHAR(30) NULL,
  password VARCHAR(128) NULL,
  email VARCHAR(40) NULL,
  status INT NULL,
  PRIMARY KEY (employee_id),
  CONSTRAINT FK_employee_role
    FOREIGN KEY (role_id)
    REFERENCES role (role_id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT FK_employee_store
    FOREIGN KEY (store_id)
    REFERENCES store (store_id)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
	ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

INSERT INTO employee (name, last_name1,last_name2,address,telephone,email,username,password,role_id,store_id,status) VALUES ('Jhonatan','Tirado','Tirado','Av Bolognesi 123','900010000','JhonatanTirado1@gmail.com','Jhonatan.Tirado1','P@ssw0rd1',1,1,1);
INSERT INTO employee (name, last_name1,last_name2,address,telephone,email,username,password,role_id,store_id,status) VALUES ('Richard','Fernandez','Cruzado','Av Bolognesi 124','900010001','RichardFernandez2@gmail.com','Richard.Fernandez2','P@ssw0rd2',1,1,1);
INSERT INTO employee (name, last_name1,last_name2,address,telephone,email,username,password,role_id,store_id,status) VALUES ('Gustavo','Osorio','Tello','Av Bolognesi 125','900010002','GustavoOsorio3@gmail.com','Gustavo.Osorio3','P@ssw0rd3',1,1,1);
INSERT INTO employee (name, last_name1,last_name2,address,telephone,email,username,password,role_id,store_id,status) VALUES ('Richar','Fernandez','Vilchez','Av Bolognesi 126','900010003','RicharFernandez4@gmail.com','Richar.Fernandez4','P@ssw0rd4',1,1,1);
INSERT INTO employee (name, last_name1,last_name2,address,telephone,email,username,password,role_id,store_id,status) VALUES ('Pedro','Mendoza','Lopez','Av Bolognesi 127','900010004','PedroMendoza5@gmail.com','Pedro.Mendoza5','P@ssw0rd5',11,1,1);
INSERT INTO employee (name, last_name1,last_name2,address,telephone,email,username,password,role_id,store_id,status) VALUES ('Julio','Saavedra','Sanchez','Av Bolognesi 128','900010005','JulioSaavedra6@gmail.com','Julio.Saavedra6','P@ssw0rd6',21,1,1);
