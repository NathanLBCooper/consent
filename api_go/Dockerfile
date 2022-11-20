FROM golang:1.18.3-alpine3.16 as build
WORKDIR /workdir
COPY go.mod ./
RUN go mod download
COPY . . 
RUN mkdir /build
RUN go build -o ./build ./cmd/server

FROM alpine:3.16 as final
WORKDIR /workdir
COPY --from=build /workdir/cmd/server/config.yaml .
COPY --from=build /workdir/build .
ENTRYPOINT [ "./build" ]
EXPOSE 3001
CMD [ "" ]
