use face;CREATE TABLE `noidloghis` (
  `noidloghis` int(11) NOT NULL AUTO_INCREMENT,
  `idcardno` varchar(45) NOT NULL,
  `capturephoto` json NOT NULL,
  `compared` tinyint(1) NOT NULL,
  `result` smallint(2) NOT NULL COMMENT '0-no result,1-success,2-failure,3-uncertainty',
  `businessnumber` varchar(45) NOT NULL,
  `stamp` datetime NOT NULL,
  PRIMARY KEY (`noidloghis`),
  UNIQUE KEY `noidloghis` (`noidloghis`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
