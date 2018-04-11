use intranetface;
CREATE TABLE `person` (
  `idperson` int(11) NOT NULL AUTO_INCREMENT,
  `idcardno` varchar(45) NOT NULL,
  `name` varchar(45) NOT NULL,
  `nation` varchar(45) NOT NULL DEFAULT '民族',
  `birthday` varchar(10) NOT NULL,
  `nationality` varchar(45) NOT NULL DEFAULT '国籍',
  `address` varchar(200) NOT NULL,
  `startdate` varchar(45) NOT NULL,
  `enddate` varchar(45) NOT NULL,
  `info` json NOT NULL,
  `issuer` varchar(45) NOT NULL,
  `gender` varchar(10) NOT NULL,
  PRIMARY KEY (`idperson`),
  UNIQUE KEY `idperson_UNIQUE` (`idperson`),
  UNIQUE KEY `idcardno_UNIQUE` (`idcardno`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;
CREATE TABLE `trails` (
  `idtrails` int(11) NOT NULL AUTO_INCREMENT,
  `time_stamp` datetime NOT NULL,
  `address` varchar(200) NOT NULL,
  `operatingagency` varchar(200) NOT NULL,
  `info` json NOT NULL,
  `idcardno` varchar(45) NOT NULL,
  PRIMARY KEY (`idtrails`),
  UNIQUE KEY `idtrails_UNIQUE` (`idtrails`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8;
