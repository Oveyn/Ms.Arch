# Apply k8s manifests

```console
kubectl apply -f k8s/run-app.yaml
```

# Test

```console
newman run test/Ms.Arch.postman_collection.json
```

# Delete resources

```console
kubectl delete -f k8s/run-app.yaml
```