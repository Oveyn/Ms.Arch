# Apply k8s manifests

#use helm

```console
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm repo update
helm install prometheus prometheus-community/kube-prometheus-stack -f k8s/helm/prometheus-chart/values.yaml --namespace monitoring --create-namespace --atomic
helm install postgres-s bitnami/postgresql -f k8s/helm/pg-chart/values.yaml --namespace ms-arch-hw02 --create-namespace --atomic --timeout 60s
helm install ms-arch-hw02 k8s/helm/hw-02-chart/ -f k8s/helm/hw-02-chart/values.yaml --namespace ms-arch-hw02 --create-namespace --atomic --timeout 60s

#apply ingress ServiceMonitor
kubectl apply -f k8s/ingress-nginx-monitoring/values.yaml
```

#If needed to add ingress Prometheus + Grafana

```console
kubectl apply -f k8s/prometheus_ingress.yaml
```

#add grafana dashboard

```console
kubectl apply -f k8s/helm/prometheus-chart/grafana_dashboard_config_map.yaml
```

# Test

```console
newman run test/Ms.Arch.postman_collection.json
```
#load test
```console
ab -t 30 -c 5 http://arch.homework/users
```

#Watch Grafana

```console
username = admin
password = prom-operator
http://graf.monitoring.mk/d/98dc7f99-b474-4ed6-a984-c504c9167ea3/hw02?orgId=1&refresh=5s&from=now-15m&to=now
```

# Delete resources
```console
kubectl delete namespace ms-arch-hw02
kubectl delete namespace monitoring
or
helm uninstall ms-arch-hw02 -n ms-arch-hw02
helm uninstall postgres-s -n ms-arch-hw02
helm uninstall prometheus -n monitoring

+ delete pvc 
kubectl delete -n ms-arch-hw02 persistentvolumeclaim data-postgres-s-postgresql-0
```

#if manual delete manifests
```console
kubectl delete -f k8s/manual-manifest/postgres.yaml -f k8s/manual-manifest/run-app.yaml
```
