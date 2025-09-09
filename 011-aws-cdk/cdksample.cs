#!/usr/bin/env dotnet

#:sdk Microsoft.NET.Sdk.Web
#:property PublishAot=False
#:package Amazon.CDK.Lib@2.214.0

// Prerequisites
// NodeJS LTS, Python 3.x LTS, Podman

// Install CDK related components
// npm i -g aws-cdk-local aws-cdk@2
// pip install awscli-local

// Launch Localstack Container (Free)
// podman run --rm --name localstack -dt -v /var/run/docker.sock:/var/run/docker.sock -p 4566:4566 -p 4510-4559:4510-4559 localstack/localstack

// Bootstrap and Deploy
// cdklocal bootstrap
// cdklocal deploy

// Lookup deployment results
// awslocal s3 ls
// awslocal sqs list-queues

using Amazon.CDK;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.SQS;
using Constructs;

var app = new App();
_ = new MinimalStack(app, "MinimalStack");
app.Synth();

public class MinimalStack : Stack
{
    public MinimalStack(Construct scope, string id, IStackProps? props = null) : base(scope, id, props)
    {
        var bucket = new Bucket(this, "DemoBucket", new BucketProps
        {
            RemovalPolicy = RemovalPolicy.DESTROY,
            AutoDeleteObjects = true
        });

        var queue = new Queue(this, "DemoQueue");

        _ = new CfnOutput(this, "BucketName", new CfnOutputProps { Value = bucket.BucketName });
        _ = new CfnOutput(this, "QueueUrl", new CfnOutputProps { Value = queue.QueueUrl });
    }
}
