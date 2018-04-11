use face;CREATE TABLE `organization` (
  `idorganization` int(11) NOT NULL AUTO_INCREMENT,
  `businessnumber` varchar(45) NOT NULL,
  `name` varchar(45) NOT NULL,
  `address` varchar(200) NOT NULL,
  PRIMARY KEY (`idorganization`),
  UNIQUE KEY `idorganization_UNIQUE` (`idorganization`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
