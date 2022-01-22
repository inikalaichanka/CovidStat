import Vue from 'vue'
import App from './App.vue'
import ArrivalsHub from './hubs/arrivas-hub'

Vue.config.productionTip = false
Vue.use(ArrivalsHub)

new Vue({
  render: h => h(App),
}).$mount('#app')
