use fm;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: fm
-- ------------------------------------------------------
-- Server version	5.7.20

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `counties`
--

DROP TABLE IF EXISTS `counties`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `counties` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  `comment` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `foreigner`
--

DROP TABLE IF EXISTS `foreigner`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `foreigner` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'unique identifier',
  `name` varchar(45) NOT NULL,
  `organization` int(11) DEFAULT NULL,
  `hisorg` json DEFAULT NULL,
  `address` varchar(200) DEFAULT NULL,
  `county` varchar(45) DEFAULT NULL,
  `town` varchar(45) DEFAULT NULL,
  `street` varchar(45) DEFAULT NULL,
  `passport` varchar(45) DEFAULT NULL,
  `gender` smallint(2) DEFAULT NULL,
  `birthday` varchar(45) DEFAULT NULL,
  `nation` varchar(45) DEFAULT NULL,
  `passportstatus` smallint(2) DEFAULT NULL,
  `arrivalreason` varchar(45) DEFAULT NULL,
  `arrivaltime` datetime DEFAULT NULL,
  `departurereason` varchar(45) DEFAULT NULL,
  `departuretime` blob,
  `residence` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `foreignerorganization_idx` (`organization`),
  CONSTRAINT `foreignerorganization` FOREIGN KEY (`organization`) REFERENCES `organization` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `inspectlog`
--

DROP TABLE IF EXISTS `inspectlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `inspectlog` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `time` datetime DEFAULT NULL,
  `foreigner` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `personlog_idx` (`foreigner`),
  CONSTRAINT `logperson` FOREIGN KEY (`foreigner`) REFERENCES `foreigner` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `messages`
--

DROP TABLE IF EXISTS `messages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `messages` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `time` datetime DEFAULT NULL,
  `assigner` int(11) DEFAULT NULL,
  `assignees` json DEFAULT NULL,
  `description` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `organization`
--

DROP TABLE IF EXISTS `organization`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `organization` (
  `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'unique identifier for organization',
  `name` varchar(45) NOT NULL,
  `address` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `police`
--

DROP TABLE IF EXISTS `police`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `police` (
  `id` int(11) NOT NULL,
  `name` varchar(45) DEFAULT NULL,
  `level` smallint(2) DEFAULT NULL,
  `phone` varchar(45) DEFAULT NULL,
  `token` varchar(45) DEFAULT NULL,
  `online` tinyint(1) DEFAULT NULL,
  `accesstoken` varchar(45) DEFAULT NULL,
  `county` varchar(45) DEFAULT NULL,
  `town` varchar(45) DEFAULT NULL,
  `city` varchar(45) DEFAULT NULL,
  `permission` smallint(2) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `policelog`
--

DROP TABLE IF EXISTS `policelog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `policelog` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `police` int(11) DEFAULT NULL,
  `intime` datetime DEFAULT NULL,
  `inlocation` varchar(45) DEFAULT NULL,
  `outtime` datetime DEFAULT NULL,
  `outlocation` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `logpolice_idx` (`police`),
  CONSTRAINT `logpolice` FOREIGN KEY (`police`) REFERENCES `police` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `residence`
--

DROP TABLE IF EXISTS `residence`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `residence` (
  `id` int(11) NOT NULL,
  `address` varchar(200) DEFAULT NULL,
  `foreigner` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `foresidence_idx` (`foreigner`),
  CONSTRAINT `foresidence` FOREIGN KEY (`foreigner`) REFERENCES `foreigner` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `sourcelog`
--

DROP TABLE IF EXISTS `sourcelog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sourcelog` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `source` varchar(45) DEFAULT NULL,
  `begintime` datetime DEFAULT NULL,
  `endtime` datetime DEFAULT NULL,
  `success` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `task`
--

DROP TABLE IF EXISTS `task`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `task` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `assigndate` datetime DEFAULT NULL,
  `foreigner` int(11) DEFAULT NULL,
  `police` int(11) DEFAULT NULL,
  `finishdate` varchar(45) DEFAULT NULL,
  `content` json DEFAULT NULL,
  `pictures` json DEFAULT NULL,
  `videos` json DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `taskperson_idx` (`foreigner`),
  KEY `taskpolice_idx` (`police`),
  CONSTRAINT `taskperson` FOREIGN KEY (`foreigner`) REFERENCES `foreigner` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `taskpolice` FOREIGN KEY (`police`) REFERENCES `police` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `taskcounty`
--

DROP TABLE IF EXISTS `taskcounty`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `taskcounty` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `batchid` int(11) DEFAULT NULL,
  `county` int(11) DEFAULT NULL,
  `amount` int(11) DEFAULT NULL,
  `finished` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `batchcounty_idx` (`batchid`),
  KEY `fktaskcounty_idx` (`county`),
  CONSTRAINT `batchcounty` FOREIGN KEY (`batchid`) REFERENCES `tasksbatch` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fktaskcounty` FOREIGN KEY (`county`) REFERENCES `counties` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tasklog`
--

DROP TABLE IF EXISTS `tasklog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tasklog` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `assigndate` datetime DEFAULT NULL,
  `foreigner` int(11) DEFAULT NULL,
  `police` int(11) DEFAULT NULL,
  `finishdate` varchar(45) DEFAULT NULL,
  `content` json DEFAULT NULL,
  `pictures` json DEFAULT NULL,
  `videos` json DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `tasklogperson_idx` (`foreigner`),
  KEY `tasklogpolice_idx` (`police`),
  CONSTRAINT `tasklogperson` FOREIGN KEY (`foreigner`) REFERENCES `foreigner` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `tasklogpolice` FOREIGN KEY (`police`) REFERENCES `police` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tasksbatch`
--

DROP TABLE IF EXISTS `tasksbatch`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `tasksbatch` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `date` datetime DEFAULT NULL,
  `amount` int(11) DEFAULT NULL,
  `finished` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `trail`
--

DROP TABLE IF EXISTS `trail`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `trail` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `eventtime` datetime DEFAULT NULL,
  `location` varchar(45) DEFAULT NULL,
  `foreigner` int(11) DEFAULT NULL,
  `description` varchar(500) DEFAULT NULL,
  `associates` json DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `trailforeigner_idx` (`foreigner`),
  CONSTRAINT `trailforeigner` FOREIGN KEY (`foreigner`) REFERENCES `foreigner` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping events for database 'fm'
--

--
-- Dumping routines for database 'fm'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-02-28 14:04:28
