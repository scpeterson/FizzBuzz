-- Ensures app role can connect to the demo database.
GRANT ALL PRIVILEGES ON DATABASE ${dbName} TO ${appUser};
