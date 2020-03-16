<template>
  <el-table :data="tableData" style="width: 100%">
    <el-table-column type="expand">
      <template slot-scope="props">
        <el-form label-position="left" inline class="demo-table-expand">
          <el-form-item label="产品名称">
            <span>{{ props.row.proName }}</span>
          </el-form-item>
          <el-form-item label="产品编码">
            <span>{{ props.row.proCode }}</span>
          </el-form-item>
          <el-form-item label="产品基本单位">
            <span>{{ props.row.proBaseUnit }}</span>
          </el-form-item>
          <el-form-item label="创建时间">
            <span>{{ props.row.creationTime }}</span>
          </el-form-item>
          <el-form-item label="产品描述">
            <span>{{ props.row.proDesc }}</span>
          </el-form-item>
        </el-form>
      </template>
    </el-table-column>
    <el-table-column label="产品名称" prop="proName"></el-table-column>
    <el-table-column label="产品编码" prop="proCode"></el-table-column>
    <el-table-column label="产品基本单位" prop="proBaseUnit"></el-table-column>
    <el-table-column label="创建时间" prop="creationTime"></el-table-column>
    <el-table-column label="操作人" prop="realName"></el-table-column>
    <el-table-column fixed="right" label="操作" width="100">
      <template slot-scope="scope">
        <!-- <el-button @click="handleClick(scope.row)" type="text" size="small">查看</el-button> -->
        <el-button type="text" size="small" @click="onEdit(scope.row)">编辑</el-button>
      </template>
    </el-table-column>
  </el-table>
</template>
<style>
.demo-table-expand {
  font-size: 0;
}
.demo-table-expand label {
  width: 90px;
  color: #99a9bf;
}
.demo-table-expand .el-form-item {
  margin-right: 0;
  margin-bottom: 0;
  width: 50%;
}
</style>
<script>
export default {
  data() {
    return {
      form: {
        pageIndex: 0,
        pageSize: 10
      }
    };
  },
  mounted() {},
  methods: {
    onEdit(row) {}
  },
  getData() {
    //请求查询数据
    var api = "/ProInfo/SearchPageList";
    //this.form.pageIndex = pIndex;
    http.get(api, JSON.stringify(this.form), response => {
      if (response && response.success && response.data) {
        //绑定列表
        this.tableData = response.data.items;
        //总记录数
        this.total = response.data.totalCount;
      } else {
        this.tableData.clear();
      }
    });
  }
};
</script>