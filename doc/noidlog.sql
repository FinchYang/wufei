
use face;
CREATE TABLE `noidlog` (
  `idnoidlog` int(11) NOT NULL AUTO_INCREMENT,
  `idcardno` varchar(45) NOT NULL,
  `capturephoto` json NOT NULL,
  `compared` tinyint(1) DEFAULT '0',
  `result` smallint(2) DEFAULT '0' COMMENT '0-no result,1-success,2-failure,3-uncertainty',
  `businessnumber` varchar(45) NOT NULL,
  `notified` tinyint(4) DEFAULT '0',
  PRIMARY KEY (`idnoidlog`),
  UNIQUE KEY `idnoidlog_UNIQUE` (`idnoidlog`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
