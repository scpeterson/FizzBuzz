-- Creates or updates the application login role used by the demos.
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = '${appUser}') THEN
        EXECUTE format('CREATE ROLE %I LOGIN PASSWORD %L', '${appUser}', '${appPassword}');
    END IF;

    EXECUTE format('ALTER ROLE %I WITH LOGIN PASSWORD %L', '${appUser}', '${appPassword}');
END $$;
