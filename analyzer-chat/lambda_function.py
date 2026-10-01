import json
from datetime import datetime, timezone


def lambda_handler(event, context):
    print("Event received:", json.dumps(event))

    return {
        "statusCode": 200,
        "headers": {"Content-Type": "application/json"},
        "body": json.dumps({
            "message": "Hello from Lambda!",
            "timestamp": datetime.now(timezone.utc).isoformat(),
        }),
    }