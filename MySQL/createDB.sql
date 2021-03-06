-- MySQL Script generated by MySQL Workbench
-- Fri May 11 00:34:58 2018
-- Model: New Model    Version: 1.0
-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema Cinema
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema Cinema
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `Cinema` DEFAULT CHARACTER SET utf8 ;
USE `Cinema` ;

-- -----------------------------------------------------
-- Table `Cinema`.`User`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Cinema`.`User` (
  `user_id` INT NOT NULL AUTO_INCREMENT,
  `login` VARCHAR(45) NOT NULL,
  `password` VARCHAR(65) NOT NULL,
  `role` ENUM('user', 'admin') NOT NULL,
  `e-mail` VARCHAR(45) NULL,
  PRIMARY KEY (`user_id`),
  UNIQUE INDEX `login_UNIQUE` (`login` ASC))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Cinema`.`Country`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Cinema`.`Country` (
  `country_id` INT NOT NULL AUTO_INCREMENT,
  `country_name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`country_id`),
  UNIQUE INDEX `country_name_UNIQUE` (`country_name` ASC))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Cinema`.`Film`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Cinema`.`Film` (
  `film_id` INT NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(45) NOT NULL,
  `description` VARCHAR(1000) NOT NULL,
  `durability` INT NULL,
  `Country_country_id` INT NOT NULL,
  `producer` VARCHAR(100) NULL,
  `URL` VARCHAR(100) NOT NULL,
  `year` INT NULL,
  PRIMARY KEY (`film_id`),
  INDEX `fk_Film_Country1_idx` (`Country_country_id` ASC),
  UNIQUE INDEX `URL_UNIQUE` (`URL` ASC),
  CONSTRAINT `fk_Film_Country1`
    FOREIGN KEY (`Country_country_id`)
    REFERENCES `Cinema`.`Country` (`country_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Cinema`.`Session`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Cinema`.`Session` (
  `session_id` INT NOT NULL AUTO_INCREMENT,
  `Film_film_id` INT NOT NULL,
  `date` DATETIME NOT NULL,
  `cost` INT NOT NULL,
  PRIMARY KEY (`session_id`),
  INDEX `fk_Session_Film1_idx` (`Film_film_id` ASC),
  CONSTRAINT `fk_Session_Film1`
    FOREIGN KEY (`Film_film_id`)
    REFERENCES `Cinema`.`Film` (`film_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Cinema`.`Order`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Cinema`.`Order` (
  `order_id` INT NOT NULL AUTO_INCREMENT,
  `User_user_id` INT NOT NULL,
  `status` ENUM('confirmed', 'rejected', 'pending confirmation') NOT NULL,
  `Session_session_id` INT NOT NULL,
  PRIMARY KEY (`order_id`),
  INDEX `fk_Order_User1_idx` (`User_user_id` ASC),
  INDEX `fk_Order_Session1_idx` (`Session_session_id` ASC),
  CONSTRAINT `fk_Order_User1`
    FOREIGN KEY (`User_user_id`)
    REFERENCES `Cinema`.`User` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Order_Session1`
    FOREIGN KEY (`Session_session_id`)
    REFERENCES `Cinema`.`Session` (`session_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Cinema`.`Category`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Cinema`.`Category` (
  `category_id` INT NOT NULL AUTO_INCREMENT,
  `title` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`category_id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Cinema`.`Place`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Cinema`.`Place` (
  `place_id` INT NOT NULL AUTO_INCREMENT,
  `row` INT NOT NULL,
  `column` INT NOT NULL,
  `cost` INT NOT NULL,
  PRIMARY KEY (`place_id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Cinema`.`Order_has_a_place`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Cinema`.`Order_has_a_place` (
  `Order_order_id` INT NOT NULL,
  `Place_place_id` INT NOT NULL,
  PRIMARY KEY (`Order_order_id`, `Place_place_id`),
  INDEX `fk_Order_has_place_Place1_idx` (`Place_place_id` ASC),
  CONSTRAINT `fk_Order_has_place_Order1`
    FOREIGN KEY (`Order_order_id`)
    REFERENCES `Cinema`.`Order` (`order_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Order_has_place_Place1`
    FOREIGN KEY (`Place_place_id`)
    REFERENCES `Cinema`.`Place` (`place_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Cinema`.`Film_has_a_category`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Cinema`.`Film_has_a_category` (
  `Film_film_id` INT NOT NULL,
  `Category_category_id` INT NOT NULL,
  PRIMARY KEY (`Film_film_id`, `Category_category_id`),
  INDEX `fk_Film_has_category_Category1_idx` (`Category_category_id` ASC),
  CONSTRAINT `fk_Film_has_category_Film1`
    FOREIGN KEY (`Film_film_id`)
    REFERENCES `Cinema`.`Film` (`film_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Film_has_category_Category1`
    FOREIGN KEY (`Category_category_id`)
    REFERENCES `Cinema`.`Category` (`category_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

USE `Cinema` ;

-- -----------------------------------------------------
-- Placeholder table for view `Cinema`.`film_information`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Cinema`.`film_information` (`'Film id'` INT, `'Film title'` INT, `'description'` INT, `'Durability'` INT, `'Producer'` INT, `'year'` INT, `'Production'` INT, `Genres` INT);

-- -----------------------------------------------------
-- Placeholder table for view `Cinema`.`order_information`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Cinema`.`order_information` (`'ID'` INT, `'User'` INT, `'Film'` INT, `'Date'` INT, `'Status'` INT, `Cost` INT, `Places` INT);

-- -----------------------------------------------------
-- Placeholder table for view `Cinema`.`common_places`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Cinema`.`common_places` (`order_id` INT, `'current_order'` INT);

-- -----------------------------------------------------
-- procedure update_orders
-- -----------------------------------------------------

DELIMITER $$
USE `Cinema`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `update_orders`(id int)
BEGIN
	DECLARE done INT DEFAULT 0;
    DECLARE o_id INT DEFAULT 0;
	DECLARE cur CURSOR FOR (select order_id from common_places where `current_order` = id);
    DECLARE CONTINUE HANDLER FOR SQLSTATE '02000' SET done = 1;	
        
	open cur;
	FETCH cur INTO o_id;
	WHILE done = 0 DO
		update `order` set status = "rejected" where `order`.order_id = o_id;
		FETCH cur INTO o_id;
	END WHILE;	
END$$

DELIMITER ;

-- -----------------------------------------------------
-- View `Cinema`.`film_information`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Cinema`.`film_information`;
USE `Cinema`;
CREATE  OR REPLACE VIEW `film_information` AS
SELECT 
	f.film_id 'Film id',
	f.title 'Film title',
    f.description 'description',
    f.durability 'Durability',
    f.producer 'Producer',
	f.year 'year',
    (SELECT country.country_name FROM country where country.country_id = f.Country_country_id) 'Production',
	GROUP_CONCAT(DISTINCT d.title ORDER BY c.Category_category_id ASC SEPARATOR ', ') AS Genres
FROM 
	film f,
    category d
LEFT JOIN 
	film_has_a_category c 
    ON 
    d.category_id = c.Category_category_id
WHERE 
	f.film_id = c.Film_film_id
GROUP BY 
	f.film_id;

-- -----------------------------------------------------
-- View `Cinema`.`order_information`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Cinema`.`order_information`;
USE `Cinema`;
CREATE  OR REPLACE VIEW `order_information` AS
SELECT 
	f.order_id as 'ID',
    (SELECT user.login FROM user where user.user_id = f.User_user_id) 'User',
    (SELECT film.title FROM film, session where film.film_id = session.Film_film_id and session.session_id = f.Session_session_id) 'Film',
	(SELECT session.date FROM session where session.session_id = f.Session_session_id) 'Date',
	f.status 'Status',
    (SELECT sum(session.cost + place.cost)
    from session, order_has_a_place, place
    where f.Session_session_id = session.session_id and
    f.order_id = order_has_a_place.Order_order_id and
    place.place_id = order_has_a_place.Place_place_id) as Cost,
	GROUP_CONCAT(DISTINCT d.place_id ORDER BY c.Place_place_id ASC SEPARATOR ', ') AS Places
FROM 
	`order` f,
    place d
LEFT JOIN 
	order_has_a_place c 
    ON 
    d.place_id = c.Place_place_id
WHERE 
	f.order_id = c.Order_order_id
GROUP BY 
	f.order_id;

-- -----------------------------------------------------
-- View `Cinema`.`common_places`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Cinema`.`common_places`;
USE `Cinema`;
CREATE  OR REPLACE VIEW `common_places` AS
select distinct order_id , `places`.id as 'current_order'
from `order`, order_has_a_place, place,
(select place_id `places`, `order`.order_id 'id'
from `order`, order_has_a_place, place
where `order`.order_id = order_has_a_place.Order_order_id
and order_has_a_place.Place_place_id = place.place_id) as `places`
where `order`.order_id = order_has_a_place.Order_order_id
and order_has_a_place.Place_place_id = place.place_id
and `places` = place_id
and status = "pending confirmation"
and order_id <> `places`.id;
USE `Cinema`;

DELIMITER $$
USE `Cinema`$$
CREATE DEFINER = CURRENT_USER TRIGGER `Cinema`.`User_BEFORE_INSERT` BEFORE INSERT ON `User` FOR EACH ROW
BEGIN
	set NEW.login = upper(NEW.login);
END$$

USE `Cinema`$$
CREATE DEFINER = CURRENT_USER TRIGGER `Cinema`.`Film_BEFORE_INSERT` BEFORE INSERT ON `Film` FOR EACH ROW
BEGIN
	set NEW.title = upper(NEW.title);
END$$


DELIMITER ;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
