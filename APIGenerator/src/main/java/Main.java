import io.github.swagger2markup.Language;
import io.github.swagger2markup.Swagger2MarkupConfig;
import io.github.swagger2markup.Swagger2MarkupConverter;
import io.github.swagger2markup.builder.Swagger2MarkupConfigBuilder;
import io.github.swagger2markup.markup.builder.MarkupLanguage;

import java.io.File;
import java.net.URL;
import java.nio.file.Path;
import java.nio.file.Paths;

public class Main {
    public static void main(String[] args) throws Exception {

        String arg1 = null;
        String arg2 = null;
        if (args != null && args.length > 0) {
            arg1 = args[0];
        } else {
            throw new Exception("未提供参数");
        }

        if (args.length > 1) {
            arg2 = args[1];
        }

        //    输出Ascii到单文件
        Swagger2MarkupConfig config = new Swagger2MarkupConfigBuilder()
                .withMarkupLanguage(MarkupLanguage.ASCIIDOC)
                .withOutputLanguage(Language.ZH)
                //.withPathsGroupedBy(GroupBy.TAGS)
                .withGeneratedExamples()
                .withoutInlineSchema()
                .build();

        if (arg2 == null) {
            arg2 = "./docs/asciidoc";
        }

        File file = new File(arg2);
        if (!file.exists()) {
            file.mkdirs();
        }

        arg2 = arg2 + "/all";
        if (arg1.contains("http") || arg1.contains("https")) {

            URL url = new URL(arg1);
            Swagger2MarkupConverter.from(url)
                    .withConfig(config)
                    .build()
                    .toFile(Paths.get(arg2));

        } else {
            Path path = Paths.get(arg1);
            Swagger2MarkupConverter.from(path)
                    .withConfig(config)
                    .build()
                    .toFile(Paths.get(arg2));
        }

    }
}