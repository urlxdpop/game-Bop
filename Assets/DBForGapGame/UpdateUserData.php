<?php
require "DBController.php";

$id = $_POST["id"] ?? null;
$levelsOpened = $_POST["levelsOpened"] ?? "";
$minTimeForLevel = $_POST["minTimeForLevel"] ?? "";
$timeForAllLevels = $_POST["timeForAllLevels"] ?? 0;
$isSecretOpen = $_POST["isSecretOpen"] ?? "";
$numSecretInLevel = $_POST["numSecretInLevel"] ?? "";

if (!$id) {
    http_response_code(400);
    echo json_encode(['error' => 'Missing id']);
    exit;
}

$result = DBController::updateUserData(
    $id,
    $levelsOpened,
    $minTimeForLevel,
    $timeForAllLevels,
    $isSecretOpen,
    $numSecretInLevel
);

echo json_encode(["success" => $result]);