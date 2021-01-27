SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for t_configs
-- ----------------------------
DROP TABLE IF EXISTS `t_configs`;
CREATE TABLE `t_configs`  (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Name` text CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Value` mediumtext CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Records of t_configs
-- ----------------------------
INSERT INTO `t_configs` VALUES (1, 'Cors:Origins', '[\"a\",\"d\"]');
INSERT INTO `t_configs` VALUES (2, 'Api:Jwt', '   {\"Secret\": \"afd3\",\"Issuer\": \"youzack\",\"Ids\":[3,5,8]}  ');
INSERT INTO `t_configs` VALUES (3, 'Api:Jwt:Audience', 'ffff');
INSERT INTO `t_configs` VALUES (4, 'Id', '3');
INSERT INTO `t_configs` VALUES (5, 'Id', '5');

SET FOREIGN_KEY_CHECKS = 1;
