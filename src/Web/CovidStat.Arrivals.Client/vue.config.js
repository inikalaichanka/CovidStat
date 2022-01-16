module.exports = {
    devServer: {
        proxy: {
            '^/arrivals': {
                target: 'http://localhost:54249/api'
            }
        },
        port: 5002,
        host: 'localhost'
    }
}