<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="html" indent="yes" />
	<xsl:template match="/">
		<html>
			<head>
				<title>Випускники</title>
				<style>
					table {
					border-collapse: collapse;
					width: 100%;
					margin-top: 20px;
					}
					th, td {
					border: 1px solid #ddd;
					padding: 8px;
					text-align: left;
					}
					th {
					background-color: #f2f2f2;
					}
				</style>
			</head>
			<body>
				<h1>Список випускників</h1>
				<table>
					<thead>
						<tr>
							<th>ПІБ</th>
							<th>Факультет</th>
							<th>Кафедра</th>
							<th>Спеціальність</th>
							<th>Рік вступу</th>
							<th>Рік випуску</th>
							<th>Кар'єра</th>
						</tr>
					</thead>
					<tbody>
						<xsl:for-each select="graduates/graduate">
							<tr>
								<td>
									<xsl:value-of select="@fullName" />
								</td>
								<td>
									<xsl:value-of select="@faculty" />
								</td>
								<td>
									<xsl:value-of select="@department" />
								</td>
								<td>
									<xsl:value-of select="@specialty" />
								</td>
								<td>
									<xsl:value-of select="@admissionYear" />
								</td>
								<td>
									<xsl:value-of select="@graduationYear" />
								</td>
								<td>
									<xsl:for-each select="career/position">
										<p>
											<strong>Посада: </strong> <xsl:value-of select=" @role" /><br />
											<strong>Компанія: </strong> <xsl:value-of select=" @company" /><br />
											<strong>Період роботи: </strong> <xsl:value-of select=" @start" /> - <xsl:value-of select="@end" />
										</p>
									</xsl:for-each>
								</td>
							</tr>
						</xsl:for-each>
					</tbody>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
