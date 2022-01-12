FROM node:15.8.0-alpine
WORKDIR /workdir

ENV PATH /workdir/node_modules/.bin:$PATH
ENV NODE_ENV development
ENV CHOKIDAR_USEPOLLING true

COPY package.json /workdir/
COPY yarn.lock /workdir/
RUN yarn

COPY tsconfig.json /workdir/tsconfig.json
COPY public /workdir/public
COPY src /workdir/src
RUN yarn build

CMD ["yarn", "start"]
