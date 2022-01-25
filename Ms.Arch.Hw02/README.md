# Apply k8s manifests

#if manual apply manifests

```console
kubectl apply -f k8s/manual-manifest/postgres.yaml -f k8s/manual-manifest/run-app.yaml
```

#use helm

```console
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update
helm install postgres-s bitnami/postgresql -f k8s/helm/pg-chart/values.yaml --namespace ms-arch-hw02 --create-namespace --atomic --timeout 60s
helm install ms-arch-hw02 k8s/helm/hw-02-chart/ -f k8s/helm/hw-02-chart/values.yaml --namespace ms-arch-hw02 --create-namespace --atomic --timeout 60s
```

# Test

```console
newman run test/Ms.Arch.postman_collection.json
```

# Delete resources
```console
kubectl delete namespace ms-arch-hw02
or
helm uninstall ms-arch-hw02 -n ms-arch-hw02
helm uninstall postgres-s -n ms-arch-hw02

+ delete pvc 
kubectl delete -n ms-arch-hw02 persistentvolumeclaim data-postgres-s-postgresql-0
```

#if manual delete manifests
```console
kubectl delete -f k8s/manual-manifest/postgres.yaml -f k8s/manual-manifest/run-app.yaml
```
