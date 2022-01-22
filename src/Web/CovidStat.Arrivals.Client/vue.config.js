module.exports = {
    devServer: {
        proxy: {
            '^/api': {
                target: 'http://localhost:54249'
            }
        },
        port: 5002,
        host: 'localhost'
    }
}