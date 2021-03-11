/*
 Navicat Premium Data Transfer

 Source Server         : 127.0.0.1
 Source Server Type    : MySQL
 Source Server Version : 50726
 Source Host           : localhost:3306
 Source Schema         : ls

 Target Server Type    : MySQL
 Target Server Version : 50726
 File Encoding         : 65001

 Date: 11/03/2021 08:59:33
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for account_data
-- ----------------------------
DROP TABLE IF EXISTS `account_data`;
CREATE TABLE `account_data`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `password` varchar(65) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `activated` tinyint(1) NOT NULL DEFAULT 1,
  `access_level` tinyint(3) NOT NULL DEFAULT 0,
  `membership_exp` bigint(13) NOT NULL DEFAULT 0,
  `last_server` tinyint(3) NOT NULL DEFAULT -1,
  `last_ip` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '127.0.0.1',
  `code` tinyint(1) NOT NULL DEFAULT 5,
  `toll` bigint(13) NOT NULL DEFAULT 0,
  `luna` bigint(13) NOT NULL DEFAULT 0,
  `question` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `answer` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `name`(`name`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 473 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for account_rewards
-- ----------------------------
DROP TABLE IF EXISTS `account_rewards`;
CREATE TABLE `account_rewards`  (
  `uniqId` int(11) NOT NULL AUTO_INCREMENT,
  `accountId` int(11) NOT NULL,
  `added` varchar(70) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  `points` decimal(20, 0) NOT NULL DEFAULT 0,
  `received` varchar(70) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '0',
  `rewarded` tinyint(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`uniqId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for account_time
-- ----------------------------
DROP TABLE IF EXISTS `account_time`;
CREATE TABLE `account_time`  (
  `account_id` int(11) NOT NULL,
  `create_time` datetime(0) NOT NULL,
  `last_active` datetime(0) NOT NULL DEFAULT CURRENT_TIMESTAMP(0),
  `expiration_type` enum('FREE','TIMING','PERIOD') CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT 'FREE',
  `expiration_time` int(11) NOT NULL DEFAULT 0,
  `expiration_period` datetime(0) NOT NULL,
  PRIMARY KEY (`account_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for active_code
-- ----------------------------
DROP TABLE IF EXISTS `active_code`;
CREATE TABLE `active_code`  (
  `code` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for banned_ip
-- ----------------------------
DROP TABLE IF EXISTS `banned_ip`;
CREATE TABLE `banned_ip`  (
  `mask` varchar(45) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `time_end` date NULL DEFAULT NULL,
  PRIMARY KEY (`mask`) USING BTREE,
  UNIQUE INDEX `mask`(`mask`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for banned_mac
-- ----------------------------
DROP TABLE IF EXISTS `banned_mac`;
CREATE TABLE `banned_mac`  (
  `uniId` int(10) NOT NULL AUTO_INCREMENT,
  `address` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `time` timestamp(0) NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP(0),
  `details` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL DEFAULT '',
  PRIMARY KEY (`uniId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for crad_items
-- ----------------------------
DROP TABLE IF EXISTS `crad_items`;
CREATE TABLE `crad_items`  (
  `id` int(11) NOT NULL,
  `item_id` int(11) NOT NULL,
  `item_name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `item_count` int(11) NOT NULL DEFAULT 1,
  `enchant` int(11) NOT NULL DEFAULT 0,
  `acthorize` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`id`, `item_id`) USING BTREE,
  CONSTRAINT `crad_items_ibak_1` FOREIGN KEY (`id`) REFERENCES `recharge_crad` (`id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for gameservers
-- ----------------------------
DROP TABLE IF EXISTS `gameservers`;
CREATE TABLE `gameservers`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `mask` varchar(45) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `password` varchar(65) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for gateway_pay
-- ----------------------------
DROP TABLE IF EXISTS `gateway_pay`;
CREATE TABLE `gateway_pay`  (
  `pay_id` int(11) NOT NULL AUTO_INCREMENT,
  `account_name` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `pay_num` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`pay_id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for login_reward
-- ----------------------------
DROP TABLE IF EXISTS `login_reward`;
CREATE TABLE `login_reward`  (
  `account_id` int(11) NOT NULL,
  `reward_id` int(11) NOT NULL,
  `reward_index` int(11) NOT NULL,
  `lastday_reward` datetime(0) NOT NULL,
  `day_reward` datetime(0) NOT NULL,
  `birthday_reward` datetime(0) NOT NULL,
  PRIMARY KEY (`account_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for luna
-- ----------------------------
DROP TABLE IF EXISTS `luna`;
CREATE TABLE `luna`  (
  `account_id` int(11) NOT NULL,
  `luna` bigint(13) NOT NULL DEFAULT 0,
  `today_use` int(11) NOT NULL DEFAULT 0,
  `key_value` int(11) NOT NULL DEFAULT 0,
  `reward_id` int(11) NOT NULL DEFAULT 0,
  `next_updata` datetime(0) NOT NULL,
  PRIMARY KEY (`account_id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for pay_infos
-- ----------------------------
DROP TABLE IF EXISTS `pay_infos`;
CREATE TABLE `pay_infos`  (
  `account_name` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `day_pay` int(11) NOT NULL DEFAULT 0,
  PRIMARY KEY (`account_name`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for pay_rewards
-- ----------------------------
DROP TABLE IF EXISTS `pay_rewards`;
CREATE TABLE `pay_rewards`  (
  `account_name` varchar(255) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `reward_type` enum('FIRST_PAY','APPOINT_PAY','TOTAL_PAY','TOTAL_CONSUM') CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `pay_num` int(11) NOT NULL DEFAULT 0,
  `reward_id` int(11) NOT NULL DEFAULT 0,
  `read_num` int(11) NOT NULL DEFAULT 0,
  `start_time` datetime(0) NOT NULL,
  `end_time` datetime(0) NOT NULL,
  PRIMARY KEY (`account_name`, `reward_type`, `reward_id`, `start_time`, `end_time`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for player_transfers
-- ----------------------------
DROP TABLE IF EXISTS `player_transfers`;
CREATE TABLE `player_transfers`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `source_server` tinyint(3) NOT NULL,
  `target_server` tinyint(3) NOT NULL,
  `source_account_id` int(11) NOT NULL,
  `target_account_id` int(11) NOT NULL,
  `player_id` int(11) NOT NULL,
  `status` tinyint(1) NOT NULL DEFAULT 0,
  `time_added` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `time_performed` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `time_done` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `comment` text CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for recharge_crad
-- ----------------------------
DROP TABLE IF EXISTS `recharge_crad`;
CREATE TABLE `recharge_crad`  (
  `id` int(11) NOT NULL,
  `crad_id` varchar(18) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `amount` int(11) NOT NULL DEFAULT 0,
  `item_name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `state` bigint(1) NULL DEFAULT 0,
  `use_time` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `use_player` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`, `crad_id`) USING BTREE,
  INDEX `id`(`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for rechargeable_card
-- ----------------------------
DROP TABLE IF EXISTS `rechargeable_card`;
CREATE TABLE `rechargeable_card`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `card_number` varchar(18) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `card_password` varchar(18) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `num` int(3) NOT NULL,
  `used` int(1) NOT NULL DEFAULT 0,
  `use_account` varchar(18) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `use_player_name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `use_time` timestamp(0) NULL DEFAULT NULL,
  PRIMARY KEY (`id`, `card_number`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tasks
-- ----------------------------
DROP TABLE IF EXISTS `tasks`;
CREATE TABLE `tasks`  (
  `id` int(5) NOT NULL,
  `task` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `type` enum('FIXED_IN_TIME') CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `last_activation` timestamp(0) NOT NULL DEFAULT '2010-01-01 00:00:00',
  `start_time` varchar(8) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `delay` int(10) NOT NULL,
  `param` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
