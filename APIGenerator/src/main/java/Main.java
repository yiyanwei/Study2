import io.github.swagger2markup.GroupBy;
import io.github.swagger2markup.Language;
import io.github.swagger2markup.Swagger2MarkupConfig;
import io.github.swagger2markup.Swagger2MarkupConverter;
import io.github.swagger2markup.builder.Swagger2MarkupConfigBuilder;
import io.github.swagger2markup.markup.builder.MarkupLanguage;

import java.net.URL;
import java.nio.file.Paths;

public class Main {
    public static void main(String[] args) throws Exception {
        //    输出Ascii到单文件
        Swagger2MarkupConfig config = new Swagger2MarkupConfigBuilder()
                .withMarkupLanguage(MarkupLanguage.ASCIIDOC)
                .withOutputLanguage(Language.ZH)
                .withPathsGroupedBy(GroupBy.TAGS)
                .withGeneratedExamples()
                .withoutInlineSchema()
                .build();

        URL url = new URL("http://localhost:5000/swagger/xugong/swagger.json");

        Swagger2MarkupConverter.from(url)
                .withConfig(config)
                .build()
                //.toFolder(Paths.get("./docs/asciidoc/generated"));
                .toFile(Paths.get("./docs/asciidoc/generated/all"));

    }
}