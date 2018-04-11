use face;CREATE TABLE `trails` (
  `idtrails` int(11) NOT NULL AUTO_INCREMENT,
  `time_stamp` datetime NOT NULL,
  `address` varchar(200) NOT NULL,
  `operatingagency` varchar(200) NOT NULL,
  `info` json NOT NULL,
  `idcardno` varchar(45) NOT NULL,
  PRIMARY KEY (`idtrails`),
  UNIQUE KEY `idtrails_UNIQUE` (`idtrails`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8;
