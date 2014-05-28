using System;
using System.Data;
using MySql.Data.MySqlClient;
using NLog;
using NLog.Targets;

namespace GeneralTools.Log.Services
{
    [Target("ElmahDatabaseTarget")]
    public sealed class ElmahDatabaseTarget : TargetWithLayout
    {
        private const int MaxAppNameLength = 60;

        protected override void Write(LogEventInfo logEvent)
        {
            using (MySqlConnection conn = new MySqlConnection(logEvent.Properties["connectionString"].ToString()))
            {
                using (MySqlCommand command = Commands.LogError(new Guid(logEvent.Properties["ErrorId"].ToString()),
                    logEvent.Properties["Application"].ToString(),
                    logEvent.Properties["Host"].ToString(),
                    logEvent.Properties["Type"].ToString(),
                    logEvent.Properties["Source"].ToString(),
                    logEvent.Message,
                    logEvent.Properties["User"].ToString(),
                    int.Parse(logEvent.Properties["StatusCode"].ToString()),
                    DateTime.Parse(logEvent.Properties["TimeUtc"].ToString()),
                    logEvent.Properties["AllXml"].ToString()
                    ))
                {
                    command.Connection = conn;
                    conn.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        private static class Commands
        {
            public static MySqlCommand LogError(
                Guid id,
                string appName,
                string hostName,
                string typeName,
                string source,
                string message,
                string user,
                int statusCode,
                DateTime time,
                string xml)
            {
                MySqlCommand command = new MySqlCommand("elmah_LogError");
                command.CommandType = CommandType.StoredProcedure;

                MySqlParameterCollection parameters = command.Parameters;
                parameters.Add("ErrorId", MySqlDbType.String, 36).Value = id.ToString();
                parameters.Add("Application", MySqlDbType.VarChar, MaxAppNameLength).Value = appName.Substring(0, Math.Min(MaxAppNameLength, appName.Length));
                parameters.Add("Host", MySqlDbType.VarChar, 30).Value = hostName.Substring(0, Math.Min(30, hostName.Length));
                parameters.Add("Type", MySqlDbType.VarChar, 100).Value = typeName.Substring(0, Math.Min(100, typeName.Length));
                parameters.Add("Source", MySqlDbType.VarChar, 60).Value = source.Substring(0, Math.Min(60, source.Length));
                parameters.Add("Message", MySqlDbType.VarChar, 500).Value = message.Substring(0, Math.Min(500, message.Length));
                parameters.Add("User", MySqlDbType.VarChar, 50).Value = user.Substring(0, Math.Min(50, user.Length));
                parameters.Add("AllXml", MySqlDbType.Text).Value = xml;
                parameters.Add("StatusCode", MySqlDbType.Int32).Value = statusCode;
                parameters.Add("TimeUtc", MySqlDbType.DateTime).Value = time;

                return command;
            }
        }
    }
}

/*
Mit diesem Script wird die Die  Datenbank Struktur erstellt die ELMAH braucht um Fehlermeldungen zu loggen
BITTE BEACHTEN
DER NAME DES DATENBANKSCHEMA IST MUSS ANGEPASST WERDEN BEVOR DIESER SCRIPT AUSGEFÜHRT WERDEN KANN
DER EINGETRAGENE DATENBANKSCHEMA NAME IST SCHEMA_ERSETZEN
*/
/*

-- ELMAH - Error Logging Modules and Handlers for ASP.NET
-- Copyright (c) 2004-9 Atif Aziz. All rights reserved.
-- 
-- Author(s):
-- 
--    Nick Berardi, http://www.coderjournal.com
-- 
-- Licensed under the Apache License, Version 2.0 (the "License");
-- you may not use this file except in compliance with the License.
-- You may obtain a copy of the License at
-- 
--    http://www.apache.org/licenses/LICENSE-2.0
-- 
-- Unless required by applicable law or agreed to in writing, software
-- distributed under the License is distributed on an "AS IS" BASIS,
-- WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
-- See the License for the specific language governing permissions and
-- limitations under the License.

-- $Id: MySql.sql 785 2011-01-22 01:44:08Z nberardi@gmail.com $

-- ===========================================================================
-- WARNING! 
-- ---------------------------------------------------------------------------
--
-- This script is designed for MySQL 5.1 GA, the script should work with later 
-- versions of MySQL, however earlier versions of MySQL 5.0 and below may cause
-- issues.  
-- 
-- If you continue with the current setup, please report any compatibility 
-- issues you encounter over at:
-- 
-- http://code.google.com/p/elmah/issues/list
-- 
-- ===========================================================================

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL';

USE `SCHEMA_ERSETZEN`;

-- -----------------------------------------------------
-- Table `SCHEMA_ERSETZEN`.`elmah_error`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `SCHEMA_ERSETZEN`.`elmah_error` (
  `ErrorId` CHAR(36) NOT NULL ,
  `Application` VARCHAR(60) NOT NULL ,
  `Host` VARCHAR(50) NOT NULL ,
  `Type` VARCHAR(100) NOT NULL ,
  `Source` VARCHAR(60) NOT NULL ,
  `Message` VARCHAR(500) NOT NULL ,
  `User` VARCHAR(50) NOT NULL ,
  `StatusCode` INT(10) NOT NULL ,
  `TimeUtc` DATETIME NOT NULL ,
  `Sequence` INT(10) NOT NULL AUTO_INCREMENT ,
  `AllXml` TEXT NOT NULL ,
  PRIMARY KEY (`Sequence`) ,
  UNIQUE INDEX `IX_ErrorId` (`ErrorId`(8) ASC) ,
  INDEX `IX_ELMAH_Error_App_Time_Seql` (`Application`(10) ASC, `TimeUtc` DESC, `Sequence` DESC) ,
  INDEX `IX_ErrorId_App` (`ErrorId`(8) ASC, `Application`(10) ASC) )
ENGINE = MyISAM
DEFAULT CHARACTER SET = utf8
CHECKSUM = 1
DELAY_KEY_WRITE = 1
ROW_FORMAT = DYNAMIC;


DELIMITER //

USE SCHEMA_ERSETZEN//

CREATE PROCEDURE `SCHEMA_ERSETZEN`.`elmah_GetErrorXml` (
  IN Id CHAR(36),
  IN App VARCHAR(60)
)
NOT DETERMINISTIC
READS SQL DATA
BEGIN
    SELECT  `AllXml`
    FROM    `elmah_error`
    WHERE   `ErrorId` = Id AND `Application` = App;
END//

USE SCHEMA_ERSETZEN//

CREATE PROCEDURE `SCHEMA_ERSETZEN`.`elmah_GetErrorsXml` (
  IN App VARCHAR(60),
  IN PageIndex INT(10),
  IN PageSize INT(10),
  OUT TotalCount INT(10)
)
NOT DETERMINISTIC
READS SQL DATA
BEGIN
    
    SELECT  count(*) INTO TotalCount
    FROM    `elmah_error`
    WHERE   `Application` = App;

    SET @index = PageIndex * PageSize;
    SET @count = PageSize;
    SET @app = App;
    PREPARE STMT FROM '
    SELECT
        `ErrorId`,
        `Application`,
        `Host`,
        `Type`,
        `Source`,
        `Message`,
        `User`,
        `StatusCode`,
        CONCAT(`TimeUtc`, '' Z'') AS `TimeUtc`
    FROM
        `elmah_error` error
    WHERE
        `Application` = ?
    ORDER BY
        `TimeUtc` DESC,
        `Sequence` DESC
    LIMIT
        ?, ?';
    EXECUTE STMT USING @app, @index, @count;

END//

USE SCHEMA_ERSETZEN//

CREATE PROCEDURE `SCHEMA_ERSETZEN`.`elmah_LogError` (
    IN ErrorId CHAR(36), 
    IN Application varchar(60), 
    IN Host VARCHAR(30), 
    IN Type VARCHAR(100), 
    IN Source VARCHAR(60), 
    IN Message VARCHAR(500), 
    IN User VARCHAR(50), 
    IN AllXml TEXT, 
    IN StatusCode INT(10), 
    IN TimeUtc DATETIME
)
NOT DETERMINISTIC
MODIFIES SQL DATA
BEGIN
    INSERT INTO `elmah_error` (
        `ErrorId`, 
        `Application`, 
        `Host`, 
        `Type`, 
        `Source`, 
        `Message`, 
        `User`, 
        `AllXml`, 
        `StatusCode`, 
        `TimeUtc`
    ) VALUES (
        ErrorId, 
        Application, 
        Host, 
        Type, 
        Source, 
        Message, 
        User, 
        AllXml, 
        StatusCode, 
        TimeUtc
    );
END//

DELIMITER ;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;



*/