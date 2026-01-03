<?php
require "DBController.php";

header('Content-Type: application/json; charset=utf-8');

$name = $_POST["name"] ?? null;
$password = $_POST["password"] ?? null;
$levelsOpened = $_POST["levelsOpened"] ?? null;
$minTimeForLevel = $_POST["minTimeForLevel"] ?? null;
$timeForAllLevels = $_POST["timeForAllLevels"] ?? null;
$isSecretOpen = $_POST["isSecretOpen"] ?? null;
$numSecretInLevel = $_POST["numSecretInLevel"] ?? null;

if (!$name || !$password) {
    echo json_encode(["error" => "missing_parameters"]);
    exit;
}

if (DBController::isNameExists($name)) {
    echo json_encode(["error" => "name_taken"]);
    exit;
}

$id = DBController::createUser($name, $password, $levelsOpened, $minTimeForLevel, $timeForAllLevels, $isSecretOpen, $numSecretInLevel);

echo json_encode(["userId" => $id, "error" => null]);
