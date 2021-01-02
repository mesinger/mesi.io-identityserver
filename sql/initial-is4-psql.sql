﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE identityserver_clients (
    "Id" text NOT NULL,
    "ClientId" character varying(50) NOT NULL,
    "ClientName" character varying(100) NOT NULL,
    "AllowedGrantTypes" text NULL,
    "RequireClientSecret" boolean NOT NULL DEFAULT TRUE,
    "ClientSecrets" text NULL,
    "AccessTokenLifetime" integer NOT NULL DEFAULT 300,
    "RedirectUris" text NULL,
    "PostLogoutRedirectUris" text NULL,
    "AllowedScopes" text NULL,
    CONSTRAINT "PK_identityserver_clients" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "IX_identityserver_clients_ClientId" ON identityserver_clients ("ClientId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20201007191109_AddIndentityServerClientTable', '5.0.1');

COMMIT;

START TRANSACTION;

ALTER TABLE identityserver_clients ADD "IsEnabled" boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20201007200238_AddEnabledToClientTable', '5.0.1');

COMMIT;

START TRANSACTION;

ALTER TABLE identityserver_clients ALTER COLUMN "Id" TYPE integer;
ALTER TABLE identityserver_clients ALTER COLUMN "Id" DROP DEFAULT;
ALTER TABLE identityserver_clients ALTER COLUMN "Id" ADD GENERATED BY DEFAULT AS IDENTITY;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20201202231814_UpdatePrimaryKeyTypeOnClientTable', '5.0.1');

COMMIT;

