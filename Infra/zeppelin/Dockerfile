FROM dylanmei/zeppelin

RUN mkdir -p /tmp/
RUN curl -sL https://dev.mysql.com/get/Downloads/Connector-J/mysql-connector-java-5.1.42.tar.gz | gunzip | tar x -C /tmp/
RUN cp /tmp/mysql-connector-java-5.1.42/mysql-connector-java-5.1.42-bin.jar /usr/zeppelin/interpreter/jdbc