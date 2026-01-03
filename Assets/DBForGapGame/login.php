<?php
require "DBController.php";

header('Content-Type: application/json; charset=utf-8');

$name = $_POST["name"] ?? null;
$password = $_POST["password"] ?? null;

if (!$name || !$password) {
    echo json_encode(["error" => "Missing parameters"]);
    exit;
}

$user = DBController::getUserByName($name);

if (!$user) {
    echo json_encode(["error" => "User not found"]);
    exit;
}

if (!password_verify($password, $user["password"])) {
    echo json_encode(["error" => "Wrong password"]);
    exit;
}

echo json_encode([
    "success" => true,
    "userId" => $user["id"]
]);
