FROM node:12.0 as develop
RUN apt-get update && apt-get install -y vim
EXPOSE 4200
USER node
RUN mkdir /home/node/.npm-global
ENV PATH=/home/node/.npm-global/bin:$PATH
ENV NPM_CONFIG_PREFIX=/home/node/.npm-global
RUN npm install -g @angular/cli@8.3.19
WORKDIR /var/www/html


FROM node:12.0 as builder
RUN apt-get update && apt-get install -y vim && apt-get -y install sudo
EXPOSE 4200
USER node
RUN mkdir /home/node/.npm-global
ENV PATH=/home/node/.npm-global/bin:$PATH
ENV NPM_CONFIG_PREFIX=/home/node/.npm-global
RUN npm install -g @angular/cli@8.3.19
WORKDIR /home/node/admin
COPY CookBook.Admin.UI/ .
USER root
RUN sudo npm install
RUN ng build --prod


FROM nginx:alpine as prod
EXPOSE 80
COPY CookBook.Admin.UI/nginx.conf /etc/nginx/nginx.conf
WORKDIR /usr/share/nginx/html
COPY --from=builder /home/node/admin/dist/cookbook-admin/ .
# ENTRYPOINT ["npm", "run", "startdocker"]
