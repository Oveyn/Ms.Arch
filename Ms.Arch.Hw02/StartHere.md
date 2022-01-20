# Apply k8s manifests

#if manual apply manifests

```console
kubectl apply -f k8s/manual-manifest/postgres.yaml -f k8s/manual-manifest/run-app.yaml
```

#if use helm

```console
helm install postgres-s bitnami/postgresql -f k8s/helm/pg-chart/values.yaml --namespace ms-arch-hw02 --create-namespace --atomic --timeout 30s
helm install ms-arch-hw02 k8s/helm/hw-02-chart/ -f k8s/helm/hw-02-chart/values.yaml --namespace ms-arch-hw02 --create-namespace --atomic --timeout 30s
```

# Test

```console
newman run test/Ms.Arch.postman_collection.json
```

# Delete resources
```console
kubectl delete namespace ms-arch-hw02
```

#if manual
```console
kubectl delete -f k8s/manual-manifest/postgres.yaml -f k8s/manual-manifest/run-app.yaml
```