<?php

class DBController
{

    private static $mysqli = null;

    private static function connect()
    {
        if (self::$mysqli === null) {
            self::$mysqli = new mysqli("localhost", "root", "", "gapgame", 3306);
            self::$mysqli->set_charset("utf8mb4");
            if (self::$mysqli->connect_error) {
                die("Connection failed: " . self::$mysqli->connect_error);
            }


        }
        return self::$mysqli;
    }

    public static function createUser($name, $password, $levelsOpened, $minTimeForLevel, $timeForAllLevels, $isSecretOpen, $numSecretInLevel)
    {
        $mysqli = self::connect();
        $hash = password_hash($password, PASSWORD_BCRYPT);

        $stmt = $mysqli->prepare("
            INSERT INTO users (name, password, levelsOpened, minTimeForLevel, timeForAllLevels, isSecretOpen, numSecretInLevel)
            VALUES (?, ?, ?, ?, ?, ?, ?)
        ");
        $stmt->bind_param("ssssiss", $name, $hash, $levelsOpened, $minTimeForLevel, $timeForAllLevels, $isSecretOpen, $numSecretInLevel);
        $stmt->execute();

        return $stmt->insert_id;
    }

    public static function getUserData($id)
    {
        $mysqli = self::connect();

        $stmt = $mysqli->prepare("
            SELECT levelsOpened, minTimeForLevel, timeForAllLevels, isSecretOpen, numSecretInLevel
            FROM users
            WHERE id = ?
        ");

        $stmt->bind_param("i", $id);
        $stmt->execute();

        return $stmt->get_result()->fetch_assoc();
    }

    public static function updateUserData($id, $levelsOpened, $minTimeForLevel, $timeForAllLevels, $isSecretOpen, $numSecretInLevel)
    {
        $mysqli = self::connect();

        $stmt = $mysqli->prepare("
            UPDATE users
            SET levelsOpened = ?,
                minTimeForLevel = ?,
                timeForAllLevels = ?,
                isSecretOpen = ?,
                numSecretInLevel = ?
            WHERE id = ?
        ");

        $stmt->bind_param(
            "ssissi",
            $levelsOpened,
            $minTimeForLevel,
            $timeForAllLevels,
            $isSecretOpen,
            $numSecretInLevel,
            $id
        );

        return $stmt->execute();
    }

    public static function isNameExists($name)
    {
        $mysqli = self::connect();
        $stmt = $mysqli->prepare("SELECT id FROM users WHERE name = ?");
        $stmt->bind_param("s", $name);
        $stmt->execute();
        $stmt->store_result();
        return $stmt->num_rows > 0;
    }


    public static function getUserByName($name)
    {
        $mysqli = self::connect();

        $stmt = $mysqli->prepare("SELECT id, password FROM users WHERE name = ?");
        $stmt->bind_param("s", $name);
        $stmt->execute();

        return $stmt->get_result()->fetch_assoc();
    }

}
