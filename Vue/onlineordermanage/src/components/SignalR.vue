<template>
  <div>
    <button id="submit">提交</button>
    <div id="result" style="color:green;font-weight:bold;"></div>
  </div>
</template>
<script>
export default {
  data() {
    return {};
  },
  mounted() {
    this.init();
  },
  methods: {
    init() {
      const signalR = require("@aspnet/signalr");
      let connection = null;
      let setupConnection = () => {
        connection = new signalR.HubConnectionBuilder()
          .withUrl("http://localhost:5002/count")
          .build();
        connection.on("someFunc", function(obj) {
          console.log(obj);
          const resultDiv = document.getElementById("result");
          resultDiv.innerHTML = "Someone called this,paramters: " + obj.random;
        });
        connection.on("ReceiveUpdate", update => {
          const resultDiv = document.getElementById("result");
          resultDiv.innerHTML = update;
        });
        connection.on("Finished", function() {
          connection.stop();
          const resultDiv = document.getElementById("result");
          resultDiv.innerHTML = "Finished";
        });
        connection.start().catch(err => console.error(err.toString()));
      };
      setupConnection();

      document.getElementById("submit").addEventListener("click", e => {
        e.preventDefault();
        fetch("http://localhost:5002/api/count/post", {
          method: "POST"
        })
          .then(response => response.text())
          .then(id => connection.invoke("GetLatestCount", id));
      });
    }
  }
};
</script>