
docker network create --driver=bridge --subnet=172.19.0.0/16 devnet


docker network create --driver=bridge --subnet=172.20.0.0/16 spark-net

docker network inspect devnet