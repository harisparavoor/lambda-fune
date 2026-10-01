import json
import os
from datetime import datetime, timezone

ENV_VARS = [
    "BEDROCK_EMBEDDING_MODEL",
    "ENVIRONMENT",
    "LAMBDA_NAME",
    "OPENSEARCH_ENDPOINT",
    "OPENSEARCH_INDEX_NAME",
    "OPENSEARCH_SECRET_NAME",
    "SERVICE_CONNECTION_CLIENT_ID",
    "SERVICE_CONNECTION_SECRET_NAME",
    "SERVICE_CONNECTION_TENANT_ID",
    "SP_AWS_REGION",
    "SP_DRIVE_IDS",
    "SP_DRIVE_NAMES",
    "SP_MONITOR_FILES",
    "SP_MONITOR_PAGES",
    "SP_SITE_ID",
    "SP_SITE_NAME",
    "SQS_QUEUE_URL",
    "TEXTRACT_NOTIFICATIONS_TABLE",
    "TEXTRACT_S3_BUCKET",
    "TEXTRACT_SNS_TOPIC_ARN",
]

# These are partially masked in the output
SENSITIVE = {
    "SERVICE_CONNECTION_CLIENT_ID",
    "SERVICE_CONNECTION_TENANT_ID",
    "SERVICE_CONNECTION_SECRET_NAME",
    "OPENSEARCH_SECRET_NAME",
}


def mask(value):
    if len(value) <= 6:
        return "***"
    return value[:3] + "***" + value[-3:]


def lambda_handler(event, context):
    found = {}
    missing = []

    for name in ENV_VARS:
        value = os.environ.get(name)
        if value is None or value == "":
            missing.append(name)
        else:
            found[name] = mask(value) if name in SENSITIVE else value

    print("Environment variables found:")
    for k, v in found.items():
        print(f"  {k} = {v}")

    if missing:
        print("MISSING environment variables:", missing)

    return {
        "statusCode": 200 if not missing else 500,
        "headers": {"Content-Type": "application/json"},
        "body": json.dumps({
            "timestamp": datetime.now(timezone.utc).isoformat(),
            "found": found,
            "missing": missing,
        }),
    }