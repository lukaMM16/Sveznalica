-- MySQL dump 10.13  Distrib 8.0.45, for Win64 (x86_64)
--
-- Host: localhost    Database: sveznalica
-- ------------------------------------------------------
-- Server version	8.0.45

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `answer_options`
--

DROP TABLE IF EXISTS `answer_options`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `answer_options` (
  `id` int NOT NULL AUTO_INCREMENT,
  `question_id` int NOT NULL,
  `text` varchar(300) COLLATE utf8mb4_unicode_ci NOT NULL,
  `is_correct` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `question_id` (`question_id`),
  CONSTRAINT `answer_options_ibfk_1` FOREIGN KEY (`question_id`) REFERENCES `questions` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=77 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `answer_options`
--

LOCK TABLES `answer_options` WRITE;
/*!40000 ALTER TABLE `answer_options` DISABLE KEYS */;
INSERT INTO `answer_options` VALUES (21,5,'Tomislav',1),(22,5,'Zvonimir',0),(23,5,'Luka',0),(24,5,'Ivan',0),(25,6,'1392',0),(26,6,'1493',0),(27,6,'1393',0),(28,6,'1492',1),(29,7,'1988',0),(30,7,'1989',1),(31,7,'1990',0),(32,7,'1978',0),(33,8,'HTTP',0),(34,8,'FTP',0),(35,8,'HTTPS',1),(36,8,'SMTP',0),(37,9,'Structured Question Language',0),(38,9,'Structured Query Language',1),(39,9,'Simple Query Language',0),(40,9,'Simple Query Language',0),(41,10,'21',0),(42,10,'25',0),(43,10,'80',1),(44,10,'443',0),(45,11,'MySQL',1),(46,11,'MangoDB',0),(47,11,'Redis',0),(48,11,'FireBase',0),(49,12,'Model View Controller',1),(50,12,'Main View Code',0),(51,12,'Model Variable Class',0),(52,12,'Master View Controller',0),(53,13,'=',0),(54,13,'==',1),(55,13,'===',0),(56,13,':=',0),(61,14,'Federer',0),(62,14,'Đoković',1),(63,14,'Nadal',0),(64,14,'Murry',0),(65,15,'10',0),(66,15,'14',0),(67,15,'8',0),(68,15,'12',1),(69,16,'Francuska',0),(70,16,'Portugal',1),(71,16,'Španjolska',0),(72,16,'Hrvatska',0),(73,17,'25 m',0),(74,17,'40 m',0),(75,17,'50 m',1),(76,17,'75 m',0);
/*!40000 ALTER TABLE `answer_options` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `attempt_answers`
--

DROP TABLE IF EXISTS `attempt_answers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `attempt_answers` (
  `id` int NOT NULL AUTO_INCREMENT,
  `attempt_id` int NOT NULL,
  `question_id` int NOT NULL,
  `selected_option_id` int DEFAULT NULL,
  `is_correct` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `uq_attempt_question` (`attempt_id`,`question_id`),
  KEY `fk_attempt_answers_question` (`question_id`),
  KEY `fk_attempt_answers_option` (`selected_option_id`),
  CONSTRAINT `fk_attempt_answers_attempt` FOREIGN KEY (`attempt_id`) REFERENCES `quiz_attempts` (`id`) ON DELETE CASCADE,
  CONSTRAINT `fk_attempt_answers_option` FOREIGN KEY (`selected_option_id`) REFERENCES `answer_options` (`id`) ON DELETE SET NULL,
  CONSTRAINT `fk_attempt_answers_question` FOREIGN KEY (`question_id`) REFERENCES `questions` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=70 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `attempt_answers`
--

LOCK TABLES `attempt_answers` WRITE;
/*!40000 ALTER TABLE `attempt_answers` DISABLE KEYS */;
INSERT INTO `attempt_answers` VALUES (1,12,5,21,1),(2,12,6,28,1),(3,12,7,30,1),(4,13,5,21,1),(5,13,6,27,0),(6,13,7,30,1),(7,15,5,21,1),(8,15,6,26,0),(9,15,7,29,0),(10,16,5,21,1),(11,16,6,28,1),(12,16,7,30,1),(13,18,5,21,1),(14,18,6,27,0),(15,18,7,32,0),(16,19,5,21,1),(17,19,6,27,0),(18,19,7,31,0),(19,21,5,22,0),(20,21,6,27,0),(21,21,7,32,0),(22,22,5,22,0),(23,22,6,28,1),(24,22,7,30,1),(25,23,5,21,1),(26,23,6,28,1),(27,23,7,30,1),(28,25,5,22,0),(29,25,6,28,1),(30,25,7,32,0),(31,26,5,21,1),(32,26,6,28,1),(33,26,7,30,1),(34,27,5,22,0),(35,27,6,28,1),(36,27,7,30,1),(37,28,5,21,1),(38,28,6,28,1),(39,28,7,30,1),(40,29,5,23,0),(41,29,6,28,1),(42,29,7,32,0),(43,31,5,23,0),(44,31,6,28,1),(45,31,7,30,1),(46,32,5,22,0),(47,32,6,25,0),(48,32,7,30,1),(49,34,5,21,1),(50,34,6,28,1),(51,34,7,30,1),(52,46,5,22,0),(53,46,6,28,1),(54,46,7,32,0),(55,47,5,21,1),(56,47,6,27,0),(57,47,7,32,0),(58,57,5,23,0),(59,57,6,28,1),(60,57,7,31,0),(61,59,5,21,1),(62,59,6,27,0),(63,59,7,32,0),(64,63,8,35,1),(65,63,9,38,1),(66,63,10,44,0),(67,63,11,45,1),(68,63,12,49,1),(69,63,13,54,1);
/*!40000 ALTER TABLE `attempt_answers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(80) COLLATE utf8mb4_unicode_ci NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categories`
--

LOCK TABLES `categories` WRITE;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
INSERT INTO `categories` VALUES (5,'Filmovi'),(6,'IT'),(3,'Povijest'),(4,'Sport');
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `challenges`
--

DROP TABLE IF EXISTS `challenges`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `challenges` (
  `id` int NOT NULL AUTO_INCREMENT,
  `quiz_id` int NOT NULL,
  `from_user_id` int NOT NULL,
  `to_user_id` int NOT NULL,
  `status` varchar(20) COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'Pending',
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `challenges`
--

LOCK TABLES `challenges` WRITE;
/*!40000 ALTER TABLE `challenges` DISABLE KEYS */;
INSERT INTO `challenges` VALUES (1,4,2,1,'Pending','2026-04-07 23:06:55'),(2,4,2,3,'Accepted','2026-04-08 22:11:14'),(3,3,2,3,'Accepted','2026-04-08 22:11:18');
/*!40000 ALTER TABLE `challenges` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `questions`
--

DROP TABLE IF EXISTS `questions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `questions` (
  `id` int NOT NULL AUTO_INCREMENT,
  `quiz_id` int NOT NULL,
  `text` varchar(500) COLLATE utf8mb4_unicode_ci NOT NULL,
  `points` int NOT NULL DEFAULT '1',
  PRIMARY KEY (`id`),
  KEY `quiz_id` (`quiz_id`),
  CONSTRAINT `questions_ibfk_1` FOREIGN KEY (`quiz_id`) REFERENCES `quizzes` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `questions`
--

LOCK TABLES `questions` WRITE;
/*!40000 ALTER TABLE `questions` DISABLE KEYS */;
INSERT INTO `questions` VALUES (5,3,'Tko je bio prvi hrvatski kralj?',1),(6,3,'Koje godine je otkrivena Amerika?',1),(7,3,'Koja je godina označila pad Berlinskog zida?',1),(8,6,'Koji protokol koristi web stranica za siguran prijenos podataka?',1),(9,6,'Što znači SQL?',1),(10,6,'Koji port koristi HTTP?',1),(11,6,'Koja baza podataka je relacijska?',1),(12,6,'Što znači MVC?',1),(13,6,'Koji operator uspoređuje jednakost u C#?',1),(14,5,'Koji tenisač ima najviše Grand Slam titula?',1),(15,5,'Koliko traje jedna četvrtina NBA utakmice?',1),(16,5,'Koja država je osvojila Euro 2016?',1),(17,5,'Kolika je duljina olimpijskog bazena?',1);
/*!40000 ALTER TABLE `questions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quiz_attempts`
--

DROP TABLE IF EXISTS `quiz_attempts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `quiz_attempts` (
  `id` int NOT NULL AUTO_INCREMENT,
  `quiz_id` int NOT NULL,
  `score` int NOT NULL DEFAULT '0',
  `started_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `finished_at` datetime DEFAULT NULL,
  `user_id` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `quiz_id` (`quiz_id`),
  KEY `fk_attempt_user` (`user_id`),
  CONSTRAINT `fk_attempt_user` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`),
  CONSTRAINT `quiz_attempts_ibfk_1` FOREIGN KEY (`quiz_id`) REFERENCES `quizzes` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=65 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `quiz_attempts`
--

LOCK TABLES `quiz_attempts` WRITE;
/*!40000 ALTER TABLE `quiz_attempts` DISABLE KEYS */;
INSERT INTO `quiz_attempts` VALUES (8,3,2,'2026-02-10 00:14:39','2026-02-10 00:14:44',2),(11,3,3,'2026-02-10 00:24:00','2026-02-10 00:24:09',1),(12,3,3,'2026-02-10 22:56:00','2026-02-10 22:56:06',1),(13,3,2,'2026-02-10 23:12:10','2026-02-10 23:12:16',1),(14,3,0,'2026-02-10 23:12:17',NULL,1),(15,3,1,'2026-02-10 23:14:07','2026-02-10 23:14:11',2),(16,3,3,'2026-02-10 23:14:46','2026-02-10 23:14:53',2),(17,3,0,'2026-02-10 23:14:54',NULL,2),(18,3,1,'2026-02-10 23:15:29','2026-02-10 23:15:31',2),(19,3,1,'2026-02-11 00:46:54','2026-02-11 00:46:58',2),(20,3,0,'2026-02-11 00:46:59',NULL,2),(21,3,0,'2026-02-11 00:47:08','2026-02-11 00:47:11',2),(22,3,2,'2026-02-11 00:54:33','2026-02-11 00:54:38',2),(23,3,3,'2026-02-11 01:06:44','2026-02-11 01:06:50',2),(24,3,0,'2026-02-11 01:06:52',NULL,2),(25,3,1,'2026-02-11 01:11:39','2026-02-11 01:11:43',2),(26,3,3,'2026-02-11 01:24:07','2026-02-11 01:24:13',2),(27,3,2,'2026-02-11 01:36:45','2026-02-11 01:36:50',2),(28,3,3,'2026-02-11 01:42:45','2026-02-11 01:42:51',2),(29,3,1,'2026-02-11 01:51:12','2026-02-11 01:51:16',2),(30,4,0,'2026-02-11 01:52:29','2026-02-11 01:52:29',1),(31,3,2,'2026-02-11 02:12:13','2026-02-11 02:12:16',2),(32,3,1,'2026-02-11 02:16:40','2026-02-11 02:16:44',2),(33,3,0,'2026-02-11 02:16:46',NULL,2),(34,3,3,'2026-02-11 08:08:45','2026-02-11 08:08:53',1),(35,4,0,'2026-04-07 20:52:31','2026-04-07 20:52:31',2),(36,4,0,'2026-04-07 20:52:35','2026-04-07 20:52:35',2),(37,4,0,'2026-04-07 20:53:08','2026-04-07 20:53:08',2),(38,3,0,'2026-04-07 20:53:58',NULL,2),(39,3,0,'2026-04-07 20:54:01',NULL,2),(40,3,0,'2026-04-07 20:54:03',NULL,2),(41,3,0,'2026-04-07 20:54:37',NULL,2),(42,3,0,'2026-04-07 20:54:39',NULL,2),(43,3,0,'2026-04-07 21:01:55',NULL,2),(44,4,0,'2026-04-07 21:01:58',NULL,2),(45,3,0,'2026-04-07 21:01:59',NULL,2),(46,3,1,'2026-04-07 21:02:12','2026-04-07 21:02:41',2),(47,3,1,'2026-04-07 21:02:51','2026-04-07 21:02:53',2),(48,3,0,'2026-04-07 21:02:53',NULL,2),(49,4,0,'2026-04-07 21:09:33',NULL,2),(50,3,0,'2026-04-07 21:09:34',NULL,2),(51,3,0,'2026-04-08 21:51:24',NULL,2),(52,3,0,'2026-04-08 22:11:34',NULL,3),(53,4,0,'2026-04-08 22:11:35',NULL,3),(54,6,0,'2026-04-08 23:04:17',NULL,2),(55,5,0,'2026-04-08 23:04:18',NULL,2),(56,4,0,'2026-04-08 23:04:19',NULL,2),(57,3,1,'2026-04-08 23:04:21','2026-04-08 23:04:26',2),(58,6,0,'2026-04-09 20:46:44',NULL,2),(59,3,1,'2026-04-09 20:46:46','2026-04-09 20:46:49',2),(60,3,0,'2026-04-09 20:46:54',NULL,2),(61,3,0,'2026-04-09 20:46:57',NULL,2),(62,3,0,'2026-04-09 20:47:01',NULL,2),(63,6,5,'2026-04-09 21:47:11','2026-04-09 21:47:38',2),(64,4,0,'2026-04-09 21:48:01',NULL,2);
/*!40000 ALTER TABLE `quiz_attempts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quizzes`
--

DROP TABLE IF EXISTS `quizzes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `quizzes` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(120) COLLATE utf8mb4_unicode_ci NOT NULL,
  `category_id` int NOT NULL,
  `difficulty` tinyint NOT NULL,
  `time_limit_sec` int NOT NULL DEFAULT '60',
  `category` varchar(100) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `category_id` (`category_id`),
  CONSTRAINT `quizzes_ibfk_1` FOREIGN KEY (`category_id`) REFERENCES `categories` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `quizzes`
--

LOCK TABLES `quizzes` WRITE;
/*!40000 ALTER TABLE `quizzes` DISABLE KEYS */;
INSERT INTO `quizzes` VALUES (3,'Kviz o povijesti',3,1,60,NULL),(4,'Opće znanje',5,2,60,NULL),(5,'Kviz o Sportu',4,2,60,NULL),(6,'IT KVIZ',6,3,120,NULL);
/*!40000 ALTER TABLE `quizzes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `team_members`
--

DROP TABLE IF EXISTS `team_members`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `team_members` (
  `id` int NOT NULL AUTO_INCREMENT,
  `team_id` int NOT NULL,
  `user_id` int NOT NULL,
  `joined_at` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `team_members`
--

LOCK TABLES `team_members` WRITE;
/*!40000 ALTER TABLE `team_members` DISABLE KEYS */;
INSERT INTO `team_members` VALUES (1,1,3,'2026-04-08 17:03:36'),(2,1,2,'2026-04-08 17:03:36');
/*!40000 ALTER TABLE `team_members` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `teams`
--

DROP TABLE IF EXISTS `teams`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `teams` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(100) COLLATE utf8mb4_unicode_ci NOT NULL,
  `owner_id` int NOT NULL,
  `created_at` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `teams`
--

LOCK TABLES `teams` WRITE;
/*!40000 ALTER TABLE `teams` DISABLE KEYS */;
INSERT INTO `teams` VALUES (1,'TimTest',3,'2026-04-08 17:03:36');
/*!40000 ALTER TABLE `teams` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `password_hash` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `role` enum('Admin','User') COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'User',
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `avatar_url` varchar(255) COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `username` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'LukaTest','ef797c8118f02dfb649607dd5d3f8c7623048c9c063d532cc95c5ed7a898a64f','User','2026-02-08 22:10:25','https://upload.wikimedia.org/wikipedia/commons/thumb/d/d2/C_Sharp_Logo_2023.svg/1280px-C_Sharp_Logo_2023.svg.png'),(2,'admin','240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9','Admin','2026-02-08 22:12:16',NULL),(3,'LukaTest2','8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92','User','2026-04-08 17:00:35',NULL);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-04-09 22:28:05
